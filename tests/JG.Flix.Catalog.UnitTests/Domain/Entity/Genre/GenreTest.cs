using Xunit;
using FluentAssertions;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.UnitTests.Domain.Entity.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
    [Fact(DisplayName =nameof(Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Instantiate()
    {
        var genreName = "Horror";
        var datetimeBefore = DateTime.Now;
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        var genre = new DomainEntity.Genre(genreName);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.Id.Should().NotBeEmpty();
        genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        genre.IsActive.Should().BeTrue();

    }

}
