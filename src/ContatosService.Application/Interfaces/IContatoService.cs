using ContatosService.Application.DTOs.Contatos;

namespace ContatosService.Application.Interfaces;

public interface IContatoService
{
    Task<ContatoDto> CriarAsync(CriarContatoDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ContatoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<ContatoDto> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Guid id, AtualizarContatoDto dto, CancellationToken cancellationToken = default);
    Task AtivarAsync(Guid id, CancellationToken cancellationToken = default);
    Task DesativarAsync(Guid id, CancellationToken cancellationToken = default);
    Task ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}
