using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTestFixtureCollection : ICollectionFixture<ListCategoriesApiTestFixture>
{ }
public class ListCategoriesApiTestFixture : CategoryBaseFixture
{
}
