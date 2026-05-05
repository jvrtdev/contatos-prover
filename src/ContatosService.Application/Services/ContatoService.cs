using ContatosService.Application.Common.Exceptions;
using ContatosService.Application.Common.Interfaces;
using ContatosService.Application.DTOs.Contatos;
using ContatosService.Application.Interfaces;
using ContatosService.Domain.Entities;
using ContatosService.Domain.Repositories;

namespace ContatosService.Application.Services;

public sealed class ContatoService : IContatoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ContatoService(IContatoRepository contatoRepository, IDateTimeProvider dateTimeProvider)
    {
        _contatoRepository = contatoRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ContatoDto> CriarAsync(CriarContatoDto dto, CancellationToken cancellationToken = default)
    {
        var contato = new Contato(dto.Nome, dto.DataNascimento, dto.Sexo, _dateTimeProvider.Hoje);

        await _contatoRepository.AdicionarAsync(contato, cancellationToken);
        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);

        return MapearParaDto(contato);
    }

    public async Task<IReadOnlyCollection<ContatoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default)
    {
        var contatos = await _contatoRepository.ListarAtivosAsync(cancellationToken);

        return contatos
            .Select(MapearParaDto)
            .ToList();
    }

    public async Task<ContatoDto> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await ObterContatoAtivoOuFalharAsync(id, cancellationToken);

        return MapearParaDto(contato);
    }

    public async Task AtualizarAsync(Guid id, AtualizarContatoDto dto, CancellationToken cancellationToken = default)
    {
        var contato = await ObterContatoAtivoOuFalharAsync(id, cancellationToken);

        contato.Atualizar(dto.Nome, dto.DataNascimento, dto.Sexo, _dateTimeProvider.Hoje);

        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }

    public async Task AtivarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await ObterContatoOuFalharAsync(id, cancellationToken);

        contato.Ativar();

        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }

    public async Task DesativarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await ObterContatoOuFalharAsync(id, cancellationToken);

        contato.Desativar();

        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await ObterContatoOuFalharAsync(id, cancellationToken);

        _contatoRepository.Remover(contato);
        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }

    private async Task<Contato> ObterContatoAtivoOuFalharAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _contatoRepository.ObterAtivoPorIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Contato não encontrado.");
    }

    private async Task<Contato> ObterContatoOuFalharAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _contatoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Contato não encontrado.");
    }

    private ContatoDto MapearParaDto(Contato contato)
    {
        return new ContatoDto(
            contato.Id,
            contato.Nome,
            contato.DataNascimento,
            contato.Sexo,
            contato.CalcularIdade(_dateTimeProvider.Hoje),
            contato.Status);
    }
}
