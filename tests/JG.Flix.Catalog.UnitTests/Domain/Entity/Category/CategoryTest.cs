using FluentAssertions;
using JG.Flix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;
    
namespace JG.Flix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        this._categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var datetimeBefore = DateTime.Now;
        
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();    
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();    
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantieteErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]    
    [InlineData(null)]    
    [InlineData(" ")]    
    public void InstantieteErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValueCategory();

        Action action = () => new DomainEntity.Category(name!, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantieteErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantieteErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();

        Action action = () => new DomainEntity.Category(validCategory.Name, null!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValueCategory();

        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characteres long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreater255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreater255Characters()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characteres long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreater10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreater10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10_000 characteres long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var categoryWithnewValues = _categoryTestFixture.GetValueCategory();

        validCategory.Update(categoryWithnewValues.Name, categoryWithnewValues.Description);

        validCategory.Name.Should().Be(categoryWithnewValues.Name);
        validCategory.Description.Should().Be(categoryWithnewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = validCategory.Description;

        validCategory.Update(newName);

        validCategory.Name.Should().Be(newName);
        validCategory.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var currentDescription = validCategory.Description;

        Action action = () => validCategory.Update(name!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var currentDescription = validCategory.Description;

        Action action = () => validCategory.Update(invalidName);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characteres long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreater255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreater255Characters()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();
        var currentDescription = validCategory.Description;

        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        Action action = () => validCategory.Update(invalidName);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characteres long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreater10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreater10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValueCategory();

        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
        {
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
        }
        Action action = () => validCategory.Update(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10_000 characteres long");
    }
}

