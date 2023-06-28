using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory: IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
{
    Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
