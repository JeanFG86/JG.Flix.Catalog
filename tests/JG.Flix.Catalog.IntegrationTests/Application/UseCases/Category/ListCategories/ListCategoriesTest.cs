using FluentAssertions;
using JG.Flix.Catalog.Infra.Data.EF;
using Xunit;
using JG.Flix.Catalog.Infra.Data.EF.Repositories;
using AppUseCases = JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using JG.Flix.Catalog.Domain.SeedWork;

namespace JG.Flix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;
[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SearhReturnsListAndTotal))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearhReturnsListAndTotal()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoryList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new AppUseCases.ListCategoriesInput(1, 20);
        var useCase = new AppUseCases.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoryList.Count);
        output.Items.Should().HaveCount(exampleCategoryList.Count);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Fact(DisplayName = nameof(SearhReturnsEmptyWhenEmpty))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearhReturnsEmptyWhenEmpty()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();        
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new AppUseCases.ListCategoriesInput(1, 20);
        var useCase = new AppUseCases.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearhReturnsPaginated))]
    [Trait("Integration/Application", "CategoryRepository - Use Cases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearhReturnsPaginated(int quantityCategoryToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoryList(quantityCategoryToGenerate);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new AppUseCases.ListCategoriesInput(page, perPage);
        var useCase = new AppUseCases.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoryList.Count);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = nameof(SearhByText))]
    [Trait("Integration/Application", "CategoryRepository - Use Cases")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 2, 2)]
    [InlineData("Horror", 2, 5, 0, 2)]
    [InlineData("Sci-fi", 1, 3, 0, 0)]
    [InlineData("Facts", 1, 5, 2, 2)]
    public async Task SearhByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityItemsTotalItems)
    {
        var cartegoryNamesList = new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Based on Real Facts",
            "Drama",
            "Drama - Based on Real Facts",
            "Comedy"
        };
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoryListWhithNames(cartegoryNamesList);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new AppUseCases.ListCategoriesInput(page, perPage, search);
        var useCase = new AppUseCases.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityItemsTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }
}