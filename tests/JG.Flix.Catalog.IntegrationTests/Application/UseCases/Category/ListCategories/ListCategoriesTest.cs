using FluentAssertions;
using JG.Flix.Catalog.Infra.Data.EF;
using Xunit;
using JG.Flix.Catalog.Infra.Data.EF.Repositories;
using AppUseCases = JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
using JG.Flix.Catalog.Application.UseCases.Category.Common;

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
}
