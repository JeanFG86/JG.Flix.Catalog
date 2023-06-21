using Bogus;
using FluentAssertions;
using JG.Flix.Catalog.Domain.Exceptions;
using JG.Flix.Catalog.Domain.Validation;
using JG.Flix.Catalog.UnitTests.Domain.Entity.Category;
using Xunit;

namespace JG.Flix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    public static IEnumerable<object[]> GetValuesLessThanMin(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 10 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var exemple = faker.Commerce.ProductName();
            var minLength = exemple.Length + (new Random().Next(1,20));
            yield return new object[]
            {
                exemple, minLength
            };
        }
    }
    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var exemple = faker.Commerce.ProductName();
            var minLength = exemple.Length - (new Random().Next(1, 5));
            yield return new object[]
            {
                exemple, minLength
            };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 5 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var exemple = faker.Commerce.ProductName();
            var maxLength = exemple.Length - (new Random().Next(1, 5));
            yield return new object[]
            {
                exemple, maxLength
            };
        }
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 7 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var exemple = faker.Commerce.ProductName();
            var maxLength = exemple.Length + (new Random().Next(1, 15));
            yield return new object[]
            {
                exemple, maxLength
            };
        }
    }

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();

        Action action = () => DomainValidation.NotNull(value, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;

        Action action = () => DomainValidation.NotNull(value, "FieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("fieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        string target = Faker.Commerce.ProductName();

        Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLength(target, minLength, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage($"fieldName should not be less than {minLength}");
    }

    [Theory(DisplayName = nameof(GetValuesGreaterThanMin))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLength(target, minLength, "fieldName");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        Action action = () => DomainValidation.MaxLength(target, maxLength, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage($"fieldName should not be greater than {maxLength}");
    }

    [Theory(DisplayName = nameof(GetValuesGreaterThanMin))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MmaxLengthOk(string target, int maxLength)
    {
        Action action = () => DomainValidation.MaxLength(target, maxLength, "fieldName");

        action.Should().NotThrow();
    }
}
