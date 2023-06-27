using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;
using Moq;
using Xunit;
using UseCases = JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;

namespace JG.Flix.Catalog.UnitTests.Application;
public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new CreateCategoryInput(
            "category Name",
            "category Description",
            true);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()),
            Times.Once);

        output.ShouldNotBeNull();
        output.Name.Should().Be("category Name");
        output.Description.Should().Be("category Description");
        output.IsActive.Should().Be(true);
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null).Should().Be(true);
    }
}
