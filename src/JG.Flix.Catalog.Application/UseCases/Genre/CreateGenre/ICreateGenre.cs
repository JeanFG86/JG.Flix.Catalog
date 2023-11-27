using JG.Flix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace JG.Flix.Catalog.Application.UseCases.Genre.CreateGenre;
public interface ICreateGenre: IRequestHandler<CreateGenreInput, GenreModelOutput>
{
}
