using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture>
{ }

public class UpdateCategoryApiTestFixture: CategoryBaseFixture
{
}
