using JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;


[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollecton : ICollectionFixture<GetCategoryTestFixture> { }

public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{    
}
