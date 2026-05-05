using ContatosService.Domain.Common;
using ContatosService.Domain.Repositories;
using ContatosService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContatosService.Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : Entity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public RepositoryBase(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> ListarAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task AdicionarAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Remover(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }
}
