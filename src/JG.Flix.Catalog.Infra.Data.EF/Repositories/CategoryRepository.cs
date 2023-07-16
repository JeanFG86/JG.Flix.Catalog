using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace JG.Flix.Catalog.Infra.Data.EF.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly FlixCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(FlixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
    {
        await _categories.AddAsync(aggregate, cancellationToken);
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }    

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
