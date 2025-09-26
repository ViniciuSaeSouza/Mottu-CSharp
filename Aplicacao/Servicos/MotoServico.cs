using Aplicacao.DTOs.Moto;
using Aplicacao.Validacoes;
using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Modelo;
using Dominio.Persistencia;
using Infraestrutura.Repositorios;

namespace Aplicacao.Servicos
{
    public class MotoServico
    {
        private readonly IRepositorio<Moto> _motoRepositorio;
        private readonly IRepositorio<Patio> _filialRepositorio;
        private readonly IRepositorioCarrapato _carrapatoRepositorio;

        public MotoServico(IRepositorio<Moto> motoRepositorio, IRepositorio<Patio> filialRepositorio,
            IRepositorioCarrapato carrapatoRepositorio)
        {
            _motoRepositorio = motoRepositorio;
            _filialRepositorio = filialRepositorio;
            _carrapatoRepositorio = carrapatoRepositorio;
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

            var filial = await ObterFilialOuLancar(dto.IdFilial);

            var carrapato = await _carrapatoRepositorio.ObterPrimeiroCarrapatoDisponivel();
            if (carrapato == null)
                throw new ExcecaoDominio("Nenhum carrapato disponível no momento.", nameof(carrapato));
            carrapato.StatusDeUso = StatusDeUsoEnum.EmUso;

            ValidarModelo(dto.Modelo);
            var moto = new Moto(dto.Placa, dto.Modelo, dto.IdFilial, dto.Chassi, filial, carrapato.Id);
            await _motoRepositorio.Adicionar(moto);

            await _carrapatoRepositorio.Atualizar(carrapato);

            return MapearParaDto(moto);
        }

        public async Task<MotoLeituraDto> Atualizar(int id, MotoAtualizarDto dto)
        {
            var moto = await ObterMotoOuLancar(id);

            ValidacaoEntidade.ValidarValor(dto.Modelo, ValidarModelo);
            ValidacaoEntidade.AlterarValor(dto.Modelo, moto.AlterarModelo);
            ValidacaoEntidade.AlterarValor(dto.Placa, moto.AlterarPlaca);
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
    }
}