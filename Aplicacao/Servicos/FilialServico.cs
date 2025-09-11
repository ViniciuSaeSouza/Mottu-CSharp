using API.Aplicacao.Repositorios;
using API.Application;
using Aplicacao.DTOs.Filial;
using Aplicacao.DTOs.Moto;
using Aplicacao.Validacoes;
using Dominio.Excecao;
using Dominio.Persistencia;

namespace Aplicacao.Servicos
{
    public class FilialServico
    {
        private readonly FilialRepositorio _filialRepositorio;
        private readonly MotoRepositorio _motoRepositorio;

        public FilialServico(FilialRepositorio filialRepositorio, MotoRepositorio motoRepositorio)
        {
            _filialRepositorio = filialRepositorio;
            _motoRepositorio = motoRepositorio;
        }

        public async Task<IEnumerable<FilialLeituraDto>> ObterTodos()
        {
            var filiais = await _filialRepositorio.ObterTodos();

            return filiais.Select(f => new FilialLeituraDto
            {
                Id = f.Id,
                Nome = f.Nome,
                Endereco = f.Endereco
            }).ToList();
        }

        public async Task<FilialLeituraDto> ObterPorId(int id)
        {
            var filial = await _filialRepositorio.ObterPorId(id);

            ValidacaoEntidade.LancarSeNulo(filial, "Filial", id);

            return new FilialLeituraDto
            {
                Id = filial.Id,
                Nome = filial.Nome,
                Endereco = filial.Endereco,
                Motos = filial.Motos.Select(m => new MotoLeituraDto
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Modelo = m.Modelo.ToString().ToUpper(),
                    NomeFilial = filial.Nome
                }).ToList()
            };
        }

        public async Task<FilialLeituraDto> Criar(FilialCriarDto dto)
        {
            // entrada inválida do user
            if (dto == null)
                throw new ExcecaoDominio("Filial não pode ser nula.", nameof(dto));

            var filial = new Filial(dto.Nome, dto.Endereco);

            await _filialRepositorio.Adicionar(filial);

            return new FilialLeituraDto
            {
                Id = filial.Id,
                Nome = filial.Nome,
                Endereco = filial.Endereco,
                Motos = new List<MotoLeituraDto>() // sempre inicializa vazio
            };
        }

        public async Task<FilialLeituraDto> Atualizar(int id, FilialAtualizarDto dto)
        {
            var filial = await _filialRepositorio.ObterPorId(id);

            ValidacaoEntidade.LancarSeNulo(filial, "Filial", id);

            ValidacaoEntidade.AlterarValor(dto.Nome, filial.AlterarNome);
            ValidacaoEntidade.AlterarValor(dto.Endereco, filial.AlterarEndereco);

            await _filialRepositorio.Atualizar(filial);

            return new FilialLeituraDto
            {
                Id = filial.Id,
                Nome = filial.Nome,
                Endereco = filial.Endereco,
                Motos = filial.Motos.Select(m => new MotoLeituraDto
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Modelo = m.Modelo.ToString().ToUpper(),
                    NomeFilial = filial.Nome
                }).ToList()
            };
        }

        public async Task Remover(int id)
        {
            var filial = await _filialRepositorio.ObterPorId(id);

            ValidacaoEntidade.LancarSeNulo(filial, "Filial", id);

            await _filialRepositorio.Remover(filial);
        }
    }
}
