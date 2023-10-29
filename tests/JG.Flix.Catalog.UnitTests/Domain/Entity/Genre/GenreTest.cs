using Xunit;
using FluentAssertions;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.UnitTests.Domain.Entity.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
    private readonly GenreTestFixture _fixture;

    public GenreTest(GenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName =nameof(Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Instantiate()
    {
        var genreName = _fixture.GetValidName();
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

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var genreName = _fixture.GetValidName();
        var datetimeBefore = DateTime.Now;
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        var genre = new DomainEntity.Genre(genreName, isActive);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.Id.Should().NotBeEmpty();
        genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        genre.IsActive.Should().Be(isActive);

    }

    [Theory(DisplayName = nameof(Activate))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Activate(bool isActive)
    {
        var genre = _fixture.GetExampleGenre(isActive);

        genre.Activate();
      
        genre.Should().NotBeNull();
        genre.Name.Should().Be(genre.Name);
        genre.Id.Should().NotBeEmpty();
        genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        genre.IsActive.Should().BeTrue();

    }

    [Theory(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Deactivate(bool isActive)
    {
        var genre = _fixture.GetExampleGenre(isActive);

        genre.Deactivate();

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genre.Name);
        genre.Id.Should().NotBeEmpty();
        genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        genre.IsActive.Should().BeFalse();

    }

}
