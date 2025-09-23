using Aplicacao.DTOs.Moto;
using Aplicacao.Validacoes;
using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Interfaces;
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

        public async Task<IEnumerable<MotoLeituraDto>> ObterTodos()
            => (await _motoRepositorio.ObterTodos())
                .Select(MapearParaDto)
                .ToList();

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
            => new()
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Modelo = moto.Modelo.ToString().ToUpper(),
                NomeFilial = moto.Patio.Nome,
                Chassi = moto.Chassi,
                Zona = moto.Zona
            };

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
    }
}
