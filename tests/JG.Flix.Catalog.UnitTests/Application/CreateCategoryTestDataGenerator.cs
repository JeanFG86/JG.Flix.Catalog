namespace JG.Flix.Catalog.UnitTests.Application;
public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();

        var invalidInputShortName = fixture.GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        invalidInputsList.Add(new object[] { invalidInputShortName, "Name should be at least 3 characteres long" });

        var invalidInputTooLongName = fixture.GetInput();
        var tooLongNameForCategory = fixture.Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {fixture.Faker.Commerce.ProductName()}";
        }
        invalidInputTooLongName.Name = tooLongNameForCategory;
        invalidInputsList.Add(new object[] { invalidInputTooLongName, "Name should be less or equal 255 characteres long" });

        var invalidInputDescriptionNull = fixture.GetInput();
        invalidInputDescriptionNull.Description = null;
        invalidInputsList.Add(new object[] { invalidInputDescriptionNull, "Description should not be null" });

        var invalidInputTooLongDescription = fixture.GetInput();
        var tooLongDescriptionForCategory = fixture.Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10_000)
        {
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {fixture.Faker.Commerce.ProductDescription()}";
        }
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
        invalidInputsList.Add(new object[] { invalidInputTooLongDescription, "Description should be less or equal 10000 characteres long" });

        return invalidInputsList;
    }
}
