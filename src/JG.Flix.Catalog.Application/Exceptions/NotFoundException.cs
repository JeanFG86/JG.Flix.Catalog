namespace JG.Flix.Catalog.Application.Exceptions;
public class NotFoundException : ApplicationException
{
    public NotFoundException(string? message) : base(message)
    {
    }
}
