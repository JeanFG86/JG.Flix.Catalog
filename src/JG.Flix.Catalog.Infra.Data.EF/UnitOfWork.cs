using JG.Flix.Catalog.Application.Interfaces;

namespace JG.Flix.Catalog.Infra.Data.EF;
public class UnitOfWork : IUnitOfWork
{
    private readonly FlixCatalogDbContext _context;

    public UnitOfWork(FlixCatalogDbContext context)
    {
        _context = context;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken) => Task.CompletedTask;
}
