namespace JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory
{
    Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
