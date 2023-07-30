using ApplicationUseCase = JG.Flix.Catalog.Application.UseCases.Category.DeleteCategory;
using JG.Flix.Catalog.Infra.Data.EF;
using JG.Flix.Catalog.Infra.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using JG.Flix.Catalog.Application.UseCases.Category.DeleteCategory;
using FluentAssertions;
using JG.Flix.Catalog.Application.Exceptions;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var categoryExample = _fixture.GetExampleCategory();
        var exampleList = _fixture.GetExampleCategoryList(10);
        await dbContext.AddRangeAsync(exampleList);
        var tracking = await dbContext.AddAsync(categoryExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);
        var useCase = new ApplicationUseCase.DeleteCategory(repository, unitOfWork);
        var input = new DeleteCategoryInput(categoryExample.Id);

        await useCase.Handle(input, CancellationToken.None);

        var asseetDbContext = _fixture.CreateDbContext(true);
        var dbCategoryDeleted = await asseetDbContext.Categories.FindAsync(categoryExample.Id);
        dbCategoryDeleted.Should().BeNull();
        asseetDbContext.Categories.ToList().Should().HaveCount(exampleList.Count);
    }

    [Fact(DisplayName = nameof(DeleteCategoryThrowsWhenNotFound))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategoryThrowsWhenNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleList = _fixture.GetExampleCategoryList(10);
        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();
        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);
        var useCase = new ApplicationUseCase.DeleteCategory(repository, unitOfWork);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
    }
}
