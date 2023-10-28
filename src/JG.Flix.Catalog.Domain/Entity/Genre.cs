
using JG.Flix.Catalog.Domain.SeedWork;

namespace JG.Flix.Catalog.Domain.Entity;
public class Genre: AggregateRoot
{    
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Genre(string name)
    {
        Name = name;
        IsActive = true;
        CreatedAt = DateTime.Now;
    }
}
