using DomainEntity = JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Application.Interfaces;
using JG.Flix.Catalog.Application.UseCases.Genre.Common;
using JG.Flix.Catalog.Domain.Repository;

namespace JG.Flix.Catalog.Application.UseCases.Genre.CreateGenre;
public class CreateGenre : ICreateGenre
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenre(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GenreModelOutput> Handle(CreateGenreInput request, CancellationToken cancellationToken)
    {
        var genre = new DomainEntity.Genre(request.Name, request.IsActive);
        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return new GenreModelOutput(genre.Id, genre.Name, genre.IsActive, genre.CreatedAt, genre.Categories);
    }
}
