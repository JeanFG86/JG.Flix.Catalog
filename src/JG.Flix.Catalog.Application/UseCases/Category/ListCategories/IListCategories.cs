using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Category.ListCategories;
public interface IListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
}
