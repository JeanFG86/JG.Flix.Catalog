﻿using FluentAssertions;
using Moq;
using Xunit;

using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.UnitTests.Application.Genre.CreateGenre;

[Collection(nameof(CreateGenreTestFixture))]
public class CreateGenreTest
{
    private readonly CreateGenreTestFixture _fixture;

    public CreateGenreTest(CreateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Create))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task Create()
    {        
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object);
        var input = _fixture.GetExampleInput();
        var datetimeBefore = DateTime.Now;

        var output = await useCase.Handle(input, CancellationToken.None);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genreRepositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Genre>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()),
            Times.Once);

        output.Should().NotBeNull();
    }
}