using ContatosService.Domain.Entities;

namespace ContatosService.Domain.Repositories;

public interface IContatoRepository : IRepositoryBase<Contato>
{
    Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Contato>> ListarAtivosAsync(CancellationToken cancellationToken = default);
}
