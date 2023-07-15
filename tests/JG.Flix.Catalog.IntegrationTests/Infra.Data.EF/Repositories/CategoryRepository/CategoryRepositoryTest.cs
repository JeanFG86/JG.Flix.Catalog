using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace JG.Flix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Insert()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var categoryRepository = new CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await dbContext.Categories.Find(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(exampleCategory.Name);  
        dbCategory.Description.Should().Be(exampleCategory.Description);  
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);  
    }
}
