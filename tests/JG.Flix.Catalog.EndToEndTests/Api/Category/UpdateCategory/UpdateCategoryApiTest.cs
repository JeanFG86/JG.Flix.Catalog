using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.Common;
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

        //var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>($"/categories/{exampleCategory.Id}");

        //response.Should().NotBeNull();
        //response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        //output.Should().BeNull();
        //var persistenceCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        //persistenceCategory.Should().BeNull();
    }
}
