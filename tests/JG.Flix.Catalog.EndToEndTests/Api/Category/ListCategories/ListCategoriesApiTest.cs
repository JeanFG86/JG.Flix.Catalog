using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using JG.Flix.Catalog.EndToEndTests.Extensions.DateTime;
using JG.Flix.Catalog.EndToEndTests.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Xunit.Abstractions;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.ListCategories;

public class Meta
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }

    public Meta(int currentPage, int perPage, int total)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
    }    
}


[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotalByDefault()
    {
        var defaultPerPage = 15;
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
 
        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Meta.Should().NotBeNull();
        output.Meta!.Total.Should().Be(exampleCategoriesList.Count);
        output.Meta.CurrentPage.Should().Be(1);
        output.Meta.PerPage.Should().Be(defaultPerPage);
        output!.Data.Should().HaveCount(defaultPerPage);
        foreach (CategoryModelOutput outputItem in output.Data!)
        {
            var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }        
    }

    [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ItemsEmptyWhenPersistenceEmpty()
    {
        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta!.Total.Should().Be(0);
        output.Data.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotal()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesInput(page: 1, perPage: 5);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        output.Meta.Total.Should().Be(exampleCategoriesList.Count);
        output.Data.Should().HaveCount(input.PerPage);
        foreach (CategoryModelOutput outputItem in output.Data!)
        {
            var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = nameof(ListPaginated))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task ListPaginated(int quantityCategoryToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoryToGenerate);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesInput(page, perPage);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        output.Meta.Total.Should().Be(exampleCategoriesList.Count);
        output.Data.Should().HaveCount(expectedQuantityItems);
        foreach (CategoryModelOutput outputItem in output.Data!)
        {
            var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 2, 2)]
    [InlineData("Horror", 2, 5, 0, 2)]
    [InlineData("Sci-fi", 1, 3, 0, 0)]
    [InlineData("Facts", 1, 5, 2, 2)]
    public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityItemsTotalItems)
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
        var exampleCategoriesList = _fixture.GetExampleCategoryListWhithNames(cartegoryNamesList);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesInput(page, perPage, search);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        output.Meta.Total.Should().Be(expectedQuantityItemsTotalItems);
        output.Data.Should().HaveCount(expectedQuantityItemsReturned);
        foreach (CategoryModelOutput outputItem in output.Data)
        {
            var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = nameof(SearhOrdered))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    public async Task SearhOrdered(string orderBy, string order)
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new ListCategoriesInput(1, 20, "", orderBy, inputOrder);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Total.Should().Be(exampleCategoriesList.Count);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        output.Data.Should().HaveCount(exampleCategoriesList.Count);

        var expectedOrderList = _fixture.CloneCategoriesListOrdered(exampleCategoriesList, input.Sort, input.Dir);

        //var count = 0;
        //var expectedArr = expectedOrderList.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
        //count = 0;
        //var outputArr = output.Items.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");

        //_output.WriteLine("Expecteds...");
        //_output.WriteLine(string.Join('\n',expectedArr));

        //_output.WriteLine("Outputs...");
        //_output.WriteLine(string.Join('\n', outputArr));

        for (int i = 0; i < expectedOrderList.Count; i++)
        {
            var outputItem = output.Data[i];
            var exampleItem = expectedOrderList[i];
            exampleItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Id.Should().Be(exampleItem!.Id);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
        }

    }

    [Theory(DisplayName = nameof(ListOrderedDates))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]    
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task ListOrderedDates(string orderBy, string order)
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new ListCategoriesInput(1, 20, "", orderBy, inputOrder);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Total.Should().Be(exampleCategoriesList.Count);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        output.Data.Should().HaveCount(exampleCategoriesList.Count);

        //var count = 0;
        //var expectedArr = expectedOrderList.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
        //count = 0;
        //var outputArr = output.Items.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");

        //_output.WriteLine("Expecteds...");
        //_output.WriteLine(string.Join('\n', expectedArr));

        //_output.WriteLine("Outputs...");
        //_output.WriteLine(string.Join('\n', outputArr));
        //DateTime? lastItemDate = null;
        foreach (CategoryModelOutput outputItem in output.Data)
        {
            var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
            //if(lastItemDate != null)
            //{
            //    _output.WriteLine(JsonConvert.SerializeObject(outputItem));
            //    if (order == "asc")
            //        Assert.True(outputItem.CreatedAt >= lastItemDate);
            //    else
            //        Assert.True(outputItem.CreatedAt <= lastItemDate);

            //}
            //lastItemDate = outputItem.CreatedAt;
        }

    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }
}
