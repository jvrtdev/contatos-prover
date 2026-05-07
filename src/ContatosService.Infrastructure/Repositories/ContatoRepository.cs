using ContatosService.Domain.Entities;
using ContatosService.Domain.Enums;
using ContatosService.Domain.Repositories;
using ContatosService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContatosService.Infrastructure.Repositories;

public sealed class ContatoRepository : RepositoryBase<Contato>, IContatoRepository
{
    public ContatoRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(
                contato => contato.Id == id && contato.Status == StatusContato.Ativo,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Contato>> ListarAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(contato => contato.Status == StatusContato.Ativo)
            .OrderBy(contato => contato.Nome)
            .ToListAsync(cancellationToken);
    }
}
