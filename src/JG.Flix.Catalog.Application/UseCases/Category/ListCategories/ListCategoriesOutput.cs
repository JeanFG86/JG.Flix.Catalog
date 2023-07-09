using JG.Flix.Catalog.Application.Common;
using JG.Flix.Catalog.Application.UseCases.Category.Common;

namespace JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
public class ListCategoriesOutput : PaginatedListOutput<CategoryModelOutput>
{
    public ListCategoriesOutput(int page, int perPage, int total, IReadOnlyList<CategoryModelOutput> items) 
        : base(page, perPage, total, items)
    {
    }
}
