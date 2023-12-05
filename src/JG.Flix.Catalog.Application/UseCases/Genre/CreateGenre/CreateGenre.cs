using DomainEntity = JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Application.UseCases.Genre.Common;
using JG.Flix.Catalog.Domain.Repository;
using JG.Flix.Catalog.Application.Exceptions;

namespace JG.Flix.Catalog.Application.UseCases.Genre.CreateGenre;
public class CreateGenre : ICreateGenre
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateGenre(IGenreRepository genreRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }
    public async Task<GenreModelOutput> Handle(CreateGenreInput request, CancellationToken cancellationToken)
    {
        var genre = new DomainEntity.Genre(request.Name, request.IsActive);
        if(request.CategoriesIds is not null)
        {
           var IdsInPersistence = await _categoryRepository.GetIdsListByIds(request.CategoriesIds, cancellationToken);
            if(IdsInPersistence.Count < request.CategoriesIds.Count) 
            {
                var notFoundIds = request.CategoriesIds.FindAll(x => !IdsInPersistence.Contains(x));
                var notFoundIdsAsString = String.Join(", ", notFoundIds);
                throw new RelatedAggregateException($"Related category id not found: {notFoundIdsAsString}");
            }
           request.CategoriesIds.ForEach(genre.AddCategory);        

        }     
        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return GenreModelOutput.FromGenre(genre);
    }
}
