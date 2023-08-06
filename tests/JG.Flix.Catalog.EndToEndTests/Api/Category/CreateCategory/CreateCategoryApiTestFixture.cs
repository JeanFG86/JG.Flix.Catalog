using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTestFixtureCollection: ICollectionFixture<CreateCategoryApiTestFixture>
{}


public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryInput getExampleInput() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandonBoolean());
}
