using JG.Flix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Category.UpdateCategory;
public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
}
