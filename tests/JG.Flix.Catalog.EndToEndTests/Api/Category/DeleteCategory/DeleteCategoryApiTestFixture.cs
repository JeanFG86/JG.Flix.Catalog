
using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;
using JG.Flix.Catalog.EndToEndTests.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTestFixtureCollection : ICollectionFixture<DeleteCategoryApiTestFixture>
{ }
public class DeleteCategoryApiTestFixture : CategoryBaseFixture
{
}
