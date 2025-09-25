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

        public MotoServico(IRepositorio<Moto> motoRepositorio, IRepositorio<Patio> filialRepositorio)
        {
            _motoRepositorio = motoRepositorio;
            _filialRepositorio = filialRepositorio;
        }

        public async Task<IResultadoPaginado<MotoLeituraDto>> ObterTodos(int pagina, int tamanhoPagina)
        {
            ValidarParametrosDePaginacao(pagina, tamanhoPagina);

            var motos = await _motoRepositorio.ObterTodos();

            var totalMotos = motos.Count();

            var motosPaginadas = motos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .Select(m => MapearParaDto(m))
                .ToList();

            return new ResultadoPaginado<MotoLeituraDto>
            {
                Items = motosPaginadas,
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                ContagemTotal = totalMotos
            };
        }

    public async Task<MotoLeituraDto> ObterPorId(int id)
            => MapearParaDto(await ObterMotoOuLancar(id));

        public async Task<MotoLeituraDto> Criar(MotoCriarDto dto)
        {
            ValidarDtoNaoNulo(dto);

            var filial = await ObterFilialOuLancar(dto.IdFilial);
            ValidarModelo(dto.Modelo);

            var moto = new Moto(dto.Placa, dto.Modelo, dto.IdFilial, dto.Chassi, filial);
            await _motoRepositorio.Adicionar(moto);

            return MapearParaDto(moto);
        }

        public async Task<MotoLeituraDto> Atualizar(int id, MotoAtualizarDto dto)
        {
            var moto = await ObterMotoOuLancar(id);

            ValidacaoEntidade.ValidarValor(dto.Modelo, ValidarModelo);
            ValidacaoEntidade.AlterarValor(dto.Modelo, moto.AlterarModelo);
            ValidacaoEntidade.AlterarValor(dto.Placa, moto.AlterarPlaca);


            if (dto.IdFilial.HasValue)
            {
                var novaFilial = await ObterFilialOuLancar(dto.IdFilial.Value);
                moto.AlterarFilial(dto.IdFilial.Value, novaFilial);
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
            => new(moto.Id, moto.Placa, moto.Modelo.ToString().ToUpper(), moto.Patio.Nome, moto.Chassi, moto.Zona);

        private async Task<Moto> ObterMotoOuLancar(int id)
        {
            var moto = await _motoRepositorio.ObterPorId(id);
            ValidacaoEntidade.LancarSeNulo(moto, "Moto", id);
            return moto;
        }

        private async Task<Patio> ObterFilialOuLancar(int id)
        {
            var filial = await _filialRepositorio.ObterPorId(id);
            ValidacaoEntidade.LancarSeNulo(filial, "Filial", id);
            return filial;
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
                throw new ExcecaoDominio("Parâmetros de paginação devem ser maiores que zero.", nameof(pagina) + ", " + nameof(tamanhoPagina));
        }
    }
}