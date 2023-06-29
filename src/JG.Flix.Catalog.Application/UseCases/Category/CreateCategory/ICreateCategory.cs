using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory: IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
{
}
