using JG.Flix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace JG.Flix.Catalog.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (int indice = 0; indice < times; indice++)
        {
            var exampleCategory = fixture.GetExampleCategory();
            var exampleInput = new UpdateCategoryInput(exampleCategory.Id, fixture.GetValidCategoryName(), fixture.GetValidCategoryDescription(), fixture.GetRandonBoolean());

            yield return new object[]
            {
                exampleCategory, exampleInput
            };

        }

    }
}
