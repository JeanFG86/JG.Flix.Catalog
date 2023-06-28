using JG.Flix.Catalog.Application.Interfaces;
using DomainEntity = JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Domain.Repository;

namespace JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);
        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return CreateCategoryOutput.FromCategory(category);
    }
}
