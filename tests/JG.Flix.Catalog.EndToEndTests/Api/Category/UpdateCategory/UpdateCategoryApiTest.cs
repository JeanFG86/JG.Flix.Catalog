﻿using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Application.UseCases.Category.UpdateCategory;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void UpdateCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = _fixture.GetExampleInput(exampleCategory.Id);

        var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>($"/categories/{exampleCategory.Id}", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Id.Should().Be(exampleCategory.Id);
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void UpdateCategoryOnlyName()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryInput(exampleCategory.Id, _fixture.GetValidCategoryName());

        var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>($"/categories/{exampleCategory.Id}", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.Id.Should().Be(exampleCategory.Id);
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive);
    }
}
