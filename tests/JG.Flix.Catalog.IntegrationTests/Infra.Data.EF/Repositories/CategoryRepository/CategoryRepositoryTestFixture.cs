using JG.Flix.Catalog.IntegrationTests.Common;
using Xunit;

namespace JG.Flix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollecton : ICollectionFixture<CategoryRepositoryTestFixture> { }

public class CategoryRepositoryTestFixture : BaseFixture
{
}
