using JG.Flix.Catalog.Application.UseCases.Category.UpdateCategory;
using JG.Flix.Catalog.EndToEndTests.Api.Category.Common;

namespace JG.Flix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture>
{ }

public class UpdateCategoryApiTestFixture : CategoryBaseFixture
{
    public UpdateCategoryInput GetExampleInput(Guid? id = null) => new(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandonBoolean()
        );

}
