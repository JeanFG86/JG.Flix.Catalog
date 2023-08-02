
using JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;
using Xunit;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture> { }

public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
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
}
