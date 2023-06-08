using JG.Flix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;
    
namespace JG.Flix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };
        var datetimeBefore = DateTime.Now;
        
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantieteErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]    
    [InlineData(null)]    
    [InlineData(" ")]    
    public void InstantieteErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantieteErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantieteErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category name", null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameLessThan3Characters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category ok Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characteres long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreater255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreater255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, "Category ok Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characteres long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreater10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreater10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category("Category name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10_000 characteres long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);
        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Category Name", "category description");
        var newValues = new { Name = "New Name", Description = "New Description" };

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Category("Category Name", "category description");
        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category Name", "category description");
        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;

        Action action = () => category.Update(name!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("Category Name", "category description");
        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;

        Action action = () => category.Update(invalidName);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characteres long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreater255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreater255Characters()
    {
        var category = new DomainEntity.Category("Category Name", "category description");
        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;

        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => category.Update(invalidName);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characteres long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreater10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreater10_000Characters()
    {
        var category = new DomainEntity.Category("Category Name", "category description");

        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        Action action = () => category.Update("Category name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10_000 characteres long", exception.Message);
    }
}

