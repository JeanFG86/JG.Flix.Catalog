using JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.Domain.SeedWork;
public interface IGenericRepository<TAggregate>: IRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
}
