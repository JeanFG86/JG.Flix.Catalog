﻿using FluentAssertions;
using Moq;
using Xunit;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;
using UseCase = JG.Flix.Catalog.Application.UseCases.Genre.CreateGenre;

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
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var useCase = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);
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
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.Categories.Should().HaveCount(0);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        (output.CreatedAt >= datetimeBefore).Should().BeTrue();
        (output.CreatedAt <= datetimeAfter).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(CreateWithRelatedCategories))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task CreateWithRelatedCategories()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var useCase = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);
        var input = _fixture.GetExampleInputWithCategories();

        var output = await useCase.Handle(input, CancellationToken.None);

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
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.Categories.Should().HaveCount(input.CategoriesIds?.Count ?? 0);
        input.CategoriesIds?.ForEach(id => output.Categories.Should().Contain(id));
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}
