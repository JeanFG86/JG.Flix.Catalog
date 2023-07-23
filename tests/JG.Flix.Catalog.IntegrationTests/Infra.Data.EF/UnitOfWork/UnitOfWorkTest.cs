using FluentAssertions;
using UnitOfWorkInfra = JG.Flix.Catalog.Infra.Data.EF;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace JG.Flix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        var dbId = Guid.NewGuid().ToString();
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoryList();
        await dbContext.AddRangeAsync(exampleCategoriesList);
        var uniOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        await uniOfWork.Commit(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();
        savedCategories.Should().HaveCount(exampleCategoriesList.Count);
    }
}
