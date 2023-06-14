using Xunit;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.UnitTests.Domain.Entity.Category;
public class CategoryTestFixture
{
    public DomainEntity.Category GetValueCategory() => new("Category Name", "category description");
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }
