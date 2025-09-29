using Aplicacao.DTOs.Moto;
using Aplicacao.Validacoes;
using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Interfaces.Mottu;
using Dominio.Modelo;
using Dominio.Persistencia;
using Dominio.Persistencia.Mottu;

namespace Aplicacao.Servicos
{
    public class MotoServico
    {
        private readonly IMotoRepositorio _motoRepositorio;
        private readonly IRepositorio<Patio> _filialRepositorio;
        private readonly IRepositorioCarrapato _carrapatoRepositorio;
        private readonly IMottuRepositorio _mottuRepositorio;

        public MotoServico(IMotoRepositorio motoRepositorio, IRepositorio<Patio> filialRepositorio,
            IRepositorioCarrapato carrapatoRepositorio, IMottuRepositorio mottuRepositorio)
        {
            _motoRepositorio = motoRepositorio;
            _filialRepositorio = filialRepositorio;
            _carrapatoRepositorio = carrapatoRepositorio;
            _mottuRepositorio = mottuRepositorio;
        }

        public async Task<List<Moto>> ObterTodos() => await _motoRepositorio.ObterTodos();

        public async Task<IResultadoPaginado<MotoLeituraDto>> ObterTodosPaginado(int pagina, int tamanhoPagina)
        {
            ValidarParametrosDePaginacao(pagina, tamanhoPagina);

            var motosPaginadas = await _motoRepositorio.ObterTodosPaginado(pagina, tamanhoPagina);

            var motosPaginadasDto = new ResultadoPaginado<MotoLeituraDto>
            {
                ContagemTotal = motosPaginadas.ContagemTotal,
                Pagina = motosPaginadas.Pagina,
                TamanhoPagina = motosPaginadas.TamanhoPagina,
                Items = motosPaginadas.Items.Select(MapearParaDto).ToList()
            };
            return motosPaginadasDto;
        }

        public async Task<MotoLeituraDto> ObterPorId(int id)
            => MapearParaDto(await ObterMotoOuLancar(id));

        public async Task<MotoLeituraDto> Criar(MotoCriarDto dto)
        {
            ValidarDtoNaoNulo(dto);

            MotoMottu? motoMottu = null;

            if (dto.Placa != null)
            {
                motoMottu = await _mottuRepositorio.ObterPorPlacaAssincrono(dto.Placa);
                if (motoMottu == null)
                    throw new ExcecaoDominio($"Não foi possível localizar uma moto com a placa {dto.Placa}",
                        nameof(dto.Placa));
            }
            else if (dto.Chassi != null)
            {
                motoMottu = await _mottuRepositorio.ObterPorChassiAssincrono(dto.Chassi);
                if (motoMottu == null)
                    throw new ExcecaoDominio($"Não foi possível localizar uma moto com o chassi: {dto.Chassi}.",
                        nameof(dto.Chassi));
            }
            else
            {   
                throw new ExcecaoDominio("É necessário informar pelo menos a placa ou o chassi da moto.", nameof(dto));
            }

            await ValidarMotoJaCadastradaAssincrono(motoMottu);

            var patio = await ObterFilialOuLancar(dto.IdPatio);

            var carrapato = await _carrapatoRepositorio.ObterPrimeiroCarrapatoDisponivel();
            if (carrapato == null)
                throw new ExcecaoDominio("Nenhum carrapato disponível no momento.", nameof(carrapato));
            carrapato.StatusDeUso = StatusDeUsoEnum.EmUso;

            var moto = new Moto(
                placa: motoMottu.Placa,
                modelo: motoMottu.Modelo,
                idPatio: dto.IdPatio,
                chassi: motoMottu.Chassi,
                patio: patio,
                idCarrapato: carrapato.Id);
            await _motoRepositorio.Adicionar(moto);

            await _carrapatoRepositorio.Atualizar(carrapato);

            return MapearParaDto(moto);
        }

        public async Task<MotoLeituraDto> Atualizar(int id, MotoAtualizarDto dto)
        {
            var moto = await ObterMotoOuLancar(id);

            ValidacaoEntidade.AlterarValor(dto.Placa, moto.AlterarPlaca);

            if (dto.Modelo != null)
            {
                moto.Modelo = dto.Modelo.Value;
                ValidarModelo(dto.Modelo.ToString()!);
            }

            if (dto.Zona != null)
            {
                moto.AlterarZona((int)dto.Zona);
            }


            if (dto.IdPatio.HasValue)
            {
                var novaFilial = await ObterFilialOuLancar(dto.IdPatio.Value);
                moto.AlterarFilial(dto.IdPatio.Value, novaFilial);
            }

            if (dto.IdCarrapato.HasValue)
            {
                moto.IdCarrapato = dto.IdCarrapato.Value;
            }

            await _motoRepositorio.Atualizar(moto);
            return MapearParaDto(moto);
        }

        public async Task Remover(int id)
        {
            var moto = await ObterMotoOuLancar(id);
            var carrapato = moto.Carrapato;
            carrapato.StatusDeUso = StatusDeUsoEnum.Disponivel;
            await _carrapatoRepositorio.Atualizar(carrapato);
            await _motoRepositorio.Remover(moto);
        }


        private MotoLeituraDto MapearParaDto(Moto moto)
            => new(moto.Id, moto.Placa, moto.Modelo.ToString().ToUpper(), moto.Patio.Nome, moto.Chassi, moto.Zona,
                moto.IdCarrapato);

        private async Task<Moto> ObterMotoOuLancar(int id)
        {
            var moto = await _motoRepositorio.ObterPorId(id);
            ValidacaoEntidade.LancarSeNulo(moto, "Moto", id);
            return moto!;
        }

        private async Task<Patio> ObterFilialOuLancar(int id)
        {
            var filial = await _filialRepositorio.ObterPorId(id);
            ValidacaoEntidade.LancarSeNulo(filial, "Filial", id);
            return filial!;
        }

        private void ValidarDtoNaoNulo(object dto)
        {
            if (dto == null)
                throw new ExcecaoDominio("Dados não podem ser nulos.", nameof(dto));
        }

        private void ValidarModelo(string modelo)
        {
            if (!Enum.IsDefined(typeof(ModeloMotoEnum), modelo.ToUpper()))
                throw new ExcecaoDominio("Modelo inválido.", nameof(modelo));
        }

        private void ValidarParametrosDePaginacao(int pagina, int tamanhoPagina)
        {
            if (pagina <= 0 || tamanhoPagina <= 0)
                throw new ExcecaoDominio("Parâmetros de paginação devem ser maiores que zero.",
                    nameof(pagina) + ", " + nameof(tamanhoPagina));
        }

        private async Task ValidarMotoJaCadastradaAssincrono(MotoMottu motoMottu)
        {
            var moto = await _motoRepositorio.ObterPorPlacaAssincrono(motoMottu.Placa);
            if (moto != null)
                throw new ExcecaoDominio($"Moto com placa {motoMottu.Placa} já cadastrada no pátio {moto.Patio.Nome}.", nameof(motoMottu.Placa));
        }
    }
}