using FluentAssertions;
using Xunit;
using JG.Flix.Catalog.Infra.Data.EF;
using Repository = JG.Flix.Catalog.Infra.Data.EF.Repositories;
using JG.Flix.Catalog.Application.Exceptions;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using JG.Flix.Catalog.Domain.Entity;

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
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleCategory.Name);  
        dbCategory.Description.Should().Be(exampleCategory.Description);  
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);  
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Get()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoryList = _fixture.GetExampleCategoryList(15);
        exampleCategoryList.Add(exampleCategory);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));        

        var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();
        await dbContext.AddRangeAsync(_fixture.GetExampleCategoryList(15));
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category {exampleId} not found.");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Update()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValeus = _fixture.GetExampleCategory();
        var exampleCategoryList = _fixture.GetExampleCategoryList(15);
        exampleCategoryList.Add(exampleCategory);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        exampleCategory.Update(newCategoryValeus.Name, newCategoryValeus.Description);
        await categoryRepository.Update(exampleCategory, CancellationToken.None);
        dbContext.SaveChanges();

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Delete()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoryList = _fixture.GetExampleCategoryList(15);
        exampleCategoryList.Add(exampleCategory);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Delete(exampleCategory, CancellationToken.None);
        dbContext.SaveChanges();

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().BeNull();       
    }

    [Fact(DisplayName = nameof(SearhReturnsListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearhReturnsListAndTotal()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoryList(15);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCategoryList.Count);
        output.Items.Should().HaveCount(exampleCategoryList.Count);

        foreach (Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }        
    }

    [Fact(DisplayName = nameof(SearhReturnsEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearhReturnsEmptyWhenPersistenceIsEmpty()
    {
        FlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);        
    }
}
