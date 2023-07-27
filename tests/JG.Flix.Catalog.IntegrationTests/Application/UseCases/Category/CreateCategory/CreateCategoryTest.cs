using JG.Flix.Catalog.Infra.Data.EF;
using JG.Flix.Catalog.Infra.Data.EF.Repositories;
using App = JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using Xunit;
using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using JG.Flix.Catalog.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;


[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext); 
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new App.CreateCategory(repository, unitOfWork);
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);        
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithName))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyWithName()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new App.CreateCategory(repository, unitOfWork);
        var input = new App.CreateCategoryInput(_fixture.GetInput().Name);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(string.Empty);
        dbCategory.IsActive.Should().Be(true);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(string.Empty);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithNameAndDescription))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyWithNameAndDescription()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new App.CreateCategory(repository, unitOfWork);
        var exampleCategory = _fixture.GetInput();
        var input = new App.CreateCategoryInput(exampleCategory.Name, exampleCategory.Description);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(true);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs), parameters: 4, MemberType = typeof(CreateCategoryTestDataGenerator))]
    public async void ThrowWhenCantInstantiateCategory(CreateCategoryInput input, string expectedExceptionMessage)
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new App.CreateCategory(repository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);   
        
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
        var dbCategoriesList = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();
        dbCategoriesList.Should().HaveCount(0);
    }
}
