using JG.Flix.Catalog.UnitTests.Common;

namespace JG.Flix.Catalog.UnitTests.Application.Genre.Common;
public class GenreUseCasesBaseFixture : BaseFixture
{
    public string GetValidGenreName() =>   
        Faker.Commerce.Categories(1)[0];
    
}
