﻿using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.ListCategories;

[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;

    public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async void ListCategoriesAndTotalByDefault()
    {
        var defaultPerPage = 15;
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
 
        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>($"/categories");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Page.Should().Be(1);
        output.PerPage.Should().Be(defaultPerPage);
        output!.Items.Should().HaveCount(defaultPerPage);
        foreach (CategoryModelOutput outputItem in output.Items)
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
    public async void ItemsEmptyWhenPersistenceEmpty()
    {
        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>($"/categories");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Total.Should().Be(0);
        output!.Items.Should().HaveCount(0);       
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async void ListCategoriesAndTotal()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesInput(page: 1, perPage: 5);

        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>($"/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output!.Items.Should().HaveCount(input.PerPage);
        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }
}
