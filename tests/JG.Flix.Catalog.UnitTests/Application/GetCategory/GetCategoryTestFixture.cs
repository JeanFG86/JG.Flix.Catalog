using JG.Flix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection: ICollectionFixture<GetCategoryTestFixture> { }

public class GetCategoryTestFixture: CategoryUseCasesBaseFixture
{
}
