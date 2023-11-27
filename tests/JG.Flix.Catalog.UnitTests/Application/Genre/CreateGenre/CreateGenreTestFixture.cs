using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Application.UseCases.Genre.CreateGenre;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.UnitTests.Application.Genre.Common;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    public CreateGenreInput GetExampleInput() 
        => new CreateGenreInput(GetValidGenreName(), GetRandonBoolean());
    

    public Mock<IGenreRepository> GetGenreRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
    => new();
}
