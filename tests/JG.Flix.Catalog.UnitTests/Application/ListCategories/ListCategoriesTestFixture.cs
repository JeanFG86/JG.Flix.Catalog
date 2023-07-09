using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.ListCategories;


[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture> { }

public class ListCategoriesTestFixture: BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();

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

    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        return Enumerable.Range(0, length)
                         .Select(_ => GetExampleCategory())
                         .ToList();
    }
}
