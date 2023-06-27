using FluentAssertions;
using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Exceptions;
using JG.Flix.Catalog.UnitTests.Domain.Entity.Category;
using Moq;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Application;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var input = _fixture.GetInput();

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

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregateAsync))]
    [Trait("Application", "CreateCategory Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async Task ThrowWhenCantInstantiateAggregateAsync(CreateCategoryInput input, string exceptionMessage)
    {
        var useCase = new CreateCategory(_fixture.GetRepositoryMock().Object, _fixture.GetUnitOfWorkMock().Object);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
    }

    public static IEnumerable<object[]> GetInvalidInputs() 
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();

        var invalidInputShortName = fixture.GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        invalidInputsList.Add(new object[] { invalidInputShortName, "Name should be at least 3 characteres long" });

        var invalidInputShortTooLongName = fixture.GetInput();
        var tooLongNameForCategory = fixture.Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {fixture.Faker.Commerce.ProductName()}";
        }
        invalidInputShortTooLongName.Name = tooLongNameForCategory;
        invalidInputsList.Add(new object[] { invalidInputShortTooLongName, "Name should be less or equal 255 characteres long" });



        return invalidInputsList;
    }
}
