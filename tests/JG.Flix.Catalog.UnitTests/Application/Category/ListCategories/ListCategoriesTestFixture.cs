﻿using JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
using JG.Flix.Catalog.Domain.SeedWork.SearchableRepository;
using JG.Flix.Catalog.UnitTests.Application.Category.Common;
using Xunit;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;

namespace JG.Flix.Catalog.UnitTests.Application.Category.ListCategories;


[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture> { }

public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
{
    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
    {
        return Enumerable.Range(0, length)
                         .Select(_ => GetExampleCategory())
                         .ToList();
    }

    public ListCategoriesInput GetExampleInput()
    {
        var randon = new Random();
        return new ListCategoriesInput(
            page: randon.Next(1, 10),
            perPage: randon.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: randon.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc);
    }
}
