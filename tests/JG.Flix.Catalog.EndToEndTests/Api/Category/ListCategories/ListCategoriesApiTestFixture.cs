using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTestFixtureCollection : ICollectionFixture<ListCategoriesApiTestFixture>
{ }
public class ListCategoriesApiTestFixture : CategoryBaseFixture
{
    public List<DomainEntity.Category> GetExampleCategoryListWhithNames(List<string> names)
    {
        return names.Select(n =>
        {
            var category = GetExampleCategory();
            category.Update(n);
            return category;
        }).ToList();
    }

    public List<DomainEntity.Category> CloneCategoriesListOrdered(List<DomainEntity.Category> categoriesList, string orderBy, SearchOrder order)
    {
        var listClone = new List<DomainEntity.Category>(categoriesList);
        var orderedEnumerable = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(n => n.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(n => n.Name),
            ("id", SearchOrder.Asc) => listClone.OrderBy(n => n.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(n => n.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(n => n.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(n => n.CreatedAt),
            _ => listClone.OrderBy(n => n.Name),
        };

        return orderedEnumerable.ThenBy(x => x.CreatedAt).ToList();
    }
}
