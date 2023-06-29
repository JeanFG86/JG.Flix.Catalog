using JG.Flix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory: IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
}
