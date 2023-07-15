﻿using JG.Flix.Catalog.UnitTests.Application.Category.CreateCategory;

namespace JG.Flix.Catalog.UnitTests.Application.Category.CreateCategory;
public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[] { fixture.GetInvalidInputShortName(), "Name should be at least 3 characteres long" });
                    break;
                case 1:
                    invalidInputsList.Add(new object[] { fixture.GetInvalidInputTooLongName(), "Name should be less or equal 255 characteres long" });
                    break;
                case 2:
                    invalidInputsList.Add(new object[] { fixture.GetInvalidInputCategoryNull(), "Description should not be null" });
                    break;
                case 3:
                    invalidInputsList.Add(new object[] { fixture.GetInvalidCategoryTooLongDescription(), "Description should be less or equal 10000 characteres long" });
                    break;
                default:
                    break;
            }
        }
        return invalidInputsList;
    }
}