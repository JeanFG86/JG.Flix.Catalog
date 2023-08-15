using JG.Flix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.Common;
public class CategoryPersistence
{
    private readonly FlixCatalogDbContext _context;

    public CategoryPersistence(FlixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntity.Category?> GetById(Guid id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task InsertList(List<DomainEntity.Category> categories)
    {
        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
    }
}
