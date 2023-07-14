using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.UnitTests.Application.Common;
using JG.Flix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }

public class DeleteCategoryTestFixture: CategoryUseCasesBaseFixture
{
    public Category GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription());
}
