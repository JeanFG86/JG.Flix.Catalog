using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTestFixtureCollection : ICollectionFixture<GetCategoryApiTestFixture>
{ }
public class GetCategoryApiTestFixture : CategoryBaseFixture
{
}
