using JG.Flix.Catalog.Domain.Entity;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListCAtegories - Use Cases")]
    public async Task List()
    {
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new ListCategoriesInput(page: 2, perPage: 15, search: "search-example", sort: "name", dir: SearchOrder.Asc);
        var outputRespositorySearch = new OutputSearch<Category>(
              currentPage: input.Page,
              perPage: input.PerPage,
              items: (IReadOnlyList<Category>)categoriesExampleList,
              total: 70
       );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.OrderBy == input.Dir
            ),
            It.IsAny<CancellationToken>()
       )).ReturnsAsync(outputRespositorySearch);
        var useCases = new ListCategories(repositoryMock.Object);

        var outPut = await useCases.Handle(input, CancellationToken.None);

        outPut.Should().NotBeNull();
        outPut.Page.Should().Be(outputRespositorySearch.CurrentPage);
        outPut.PerPage.Should().Be(outputRespositorySearch.PerPage);
        outPut.Total.Should().Be(outputRespositorySearch.Total);
        outPut.Itens.Should().HaveCount(outputRespositorySearch.Items.Count);
        outPut.Itens.Foreach(outputItem =>
        {
            var repositoryCategory = outputRespositorySearch.Items.Find(x => x.id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory.Name);
            outputItem.Description.Should().Be(repositoryCategory.Description);
            outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.OrderBy == input.Dir
            ),
            It.IsAny<CancellationToken>()
       ), Times.Once);
    }
}
