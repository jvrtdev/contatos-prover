using ContatosService.Domain.Common;

namespace ContatosService.Domain.Repositories;

public interface IRepositoryBase<TEntity>
    where TEntity : Entity
{
    Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> ListarAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remover(TEntity entity);
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}
