using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
{
    private readonly CreateCategoryApiTestFixture _fixture;

    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "")]
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task CreateCategory()
    {
        var input =  _fixture.getExampleInput();

        CategoryModelOutput output = await _fixture.Api.Post<CategoryModelOutput>("/categories", input);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}
