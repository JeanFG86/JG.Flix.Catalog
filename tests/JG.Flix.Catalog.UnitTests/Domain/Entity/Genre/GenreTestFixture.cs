using JG.Flix.Catalog.UnitTests.Common;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Domain.Entity.Genre;

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture> { }

public class GenreTestFixture : BaseFixture
{
}
