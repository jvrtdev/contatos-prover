using ContatosService.Domain.Common;

namespace ContatosService.Domain.Repositories;

public interface IRepositoryBase<TEntity>
    where TEntity : Entity
{
    Task AdicionarAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> ListarAsync(CancellationToken cancellationToken = default);
    void Remover(TEntity entity);
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}
