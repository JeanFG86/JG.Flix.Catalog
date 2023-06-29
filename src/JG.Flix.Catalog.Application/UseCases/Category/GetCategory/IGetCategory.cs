using JG.Flix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Category.GetCategory;
public interface IGetCategory: IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
}
