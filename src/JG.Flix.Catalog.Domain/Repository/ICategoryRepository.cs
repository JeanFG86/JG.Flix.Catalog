using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.SeedWork;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;

namespace JG.Flix.Catalog.Domain.Repository;
public interface ICategoryRepository: IGenericRepository<Category>, ISearchableRepository<Category>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken);
}
