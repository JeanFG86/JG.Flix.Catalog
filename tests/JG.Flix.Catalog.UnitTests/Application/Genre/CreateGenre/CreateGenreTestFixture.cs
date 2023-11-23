using JG.Flix.Catalog.UnitTests.Application.Genre.Common;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
}
