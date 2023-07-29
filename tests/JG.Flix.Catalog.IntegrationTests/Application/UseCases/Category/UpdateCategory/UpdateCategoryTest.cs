using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;
using ApplicationUseCase = JG.Flix.Catalog.Application.UseCases.Category.UpdateCategory;
using JG.Flix.Catalog.Infra.Data.EF.Repositories;
using JG.Flix.Catalog.Infra.Data.EF;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using JG.Flix.Catalog.Application.Exceptions;
using JG.Flix.Catalog.Domain.Exceptions;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategory(DomainEntity.Category exampleCategory, UpdateCategoryInput input)
    {
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleCategoryList());
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);        
    }

    [Theory(DisplayName = nameof(UpdateCategoryWhithoutIsActive))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryWhithoutIsActive(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(exampleCategory.Id, exampleCategory.Name, exampleCategory.Description);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleCategoryList());
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryOnlyName(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(exampleCategory.Id, exampleCategory.Name);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleCategoryList());
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateCategoryThrowsNotFoundCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    public async Task UpdateCategoryThrowsNotFoundCategory()
    {
        var input =_fixture.GetValidInput();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleCategoryList());
        dbContext.SaveChanges();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
    }

    [Theory(DisplayName = nameof(UpdateCategoryThrowsCantInstantiateCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs), parameters: 6, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryThrowsCantInstantiateCategory(UpdateCategoryInput input, string expectedExeptionMessage)
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategories = _fixture.GetExampleCategoryList(); 
        await dbContext.AddRangeAsync(exampleCategories);
        dbContext.SaveChanges();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);
        input.Id = exampleCategories[0].Id;

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExeptionMessage);
    }
}
