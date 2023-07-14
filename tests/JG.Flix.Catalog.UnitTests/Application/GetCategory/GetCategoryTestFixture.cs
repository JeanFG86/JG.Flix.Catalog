using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.UnitTests.Application.Common;
using JG.Flix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection: ICollectionFixture<GetCategoryTestFixture> { }

public class GetCategoryTestFixture: CategoryUseCasesBaseFixture
{
}
