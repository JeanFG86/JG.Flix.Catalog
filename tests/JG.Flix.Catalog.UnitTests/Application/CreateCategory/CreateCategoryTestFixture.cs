﻿using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.UnitTests.Application.Common;
using JG.Flix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
{
}

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    
    public CreateCategoryInput GetInput() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandonBoolean());    

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetInput();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        }
        invalidInputTooLongName.Name = tooLongNameForCategory;
        return invalidInputTooLongName;
    }

    public CreateCategoryInput GetInvalidInputCategoryNull()
    {
        var invalidInputDescriptionNull = GetInput();
        invalidInputDescriptionNull.Description = null;
        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidCategoryTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10_000)
        {
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        }
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
        return invalidInputTooLongDescription;
    }
}
