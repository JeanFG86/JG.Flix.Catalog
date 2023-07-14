using UseCase = JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using Moq;
using Xunit;
using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.Common;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.UnitTests.Application.Category.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task List()
    {
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetExampleInput();
        var outputRespositorySearch = new SearchOutput<DomainEntity.Category>(
              currentPage: input.Page,
              perPage: input.PerPage,
              total: new Random().Next(50, 200),
              items: categoriesExampleList
       );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
       )).ReturnsAsync(outputRespositorySearch);
        var useCases = new UseCase.ListCategories(repositoryMock.Object);

        var output = await useCases.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRespositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRespositorySearch.PerPage);
        output.Total.Should().Be(outputRespositorySearch.Total);
        output.Items.Should().HaveCount(outputRespositorySearch.Items.Count);
        ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = outputRespositorySearch.Items.FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory.Description);
            outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
       ), Times.Once);
    }

    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [Trait("Application", "ListCategories - Use Cases")]
    [MemberData(nameof(ListCategoriesTestDataGenerator.GetInputsWithoutAllParameters), parameters: 10, MemberType = typeof(ListCategoriesTestDataGenerator))]
    public async Task ListInputWithoutAllParameters(UseCase.ListCategoriesInput input)
    {
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var outputRespositorySearch = new SearchOutput<DomainEntity.Category>(
              currentPage: input.Page,
              perPage: input.PerPage,
              total: new Random().Next(50, 200),
              items: categoriesExampleList
       );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
       )).ReturnsAsync(outputRespositorySearch);
        var useCases = new UseCase.ListCategories(repositoryMock.Object);

        var output = await useCases.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRespositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRespositorySearch.PerPage);
        output.Total.Should().Be(outputRespositorySearch.Total);
        output.Items.Should().HaveCount(outputRespositorySearch.Items.Count);
        ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = outputRespositorySearch.Items.FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory.Description);
            outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
       ), Times.Once);
    }

    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task ListOkWhenEmpty()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetExampleInput();
        var outputRespositorySearch = new SearchOutput<DomainEntity.Category>(
              currentPage: input.Page,
              perPage: input.PerPage,
              total: 0,
              items: new List<DomainEntity.Category>().AsReadOnly()
       );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
       )).ReturnsAsync(outputRespositorySearch);
        var useCases = new UseCase.ListCategories(repositoryMock.Object);

        var output = await useCases.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRespositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRespositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
       ), Times.Once);
    }
}
