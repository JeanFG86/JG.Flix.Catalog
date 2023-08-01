
using JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture> { }

public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
{
}
