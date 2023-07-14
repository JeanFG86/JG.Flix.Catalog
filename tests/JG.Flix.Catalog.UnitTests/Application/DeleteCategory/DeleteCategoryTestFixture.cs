using JG.Flix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }

public class DeleteCategoryTestFixture: CategoryUseCasesBaseFixture
{   
}
