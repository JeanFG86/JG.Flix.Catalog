using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.Infra.Data.EF;
using JG.Flix.Catalog.IntegrationTests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JG.Flix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollecton : ICollectionFixture<CategoryRepositoryTestFixture> { }

public class CategoryRepositoryTestFixture : BaseFixture
{

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public bool GetRandonBoolean() => new Random().NextDouble() < 0.5;

    public Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandonBoolean());

    public FlixCatalogDbContext CreateDbContext()
    {
        return new FlixCatalogDbContext(new DbContextOptionsBuilder<FlixCatalogDbContext>().UseInMemoryDatabase("integration-test-db").Options);
    }
}
