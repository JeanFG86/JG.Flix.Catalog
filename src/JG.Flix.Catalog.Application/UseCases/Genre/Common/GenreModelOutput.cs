using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Flix.Catalog.Application.UseCases.Genre.Common;
public class GenreModelOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Guid> Categories { get; set; }

    public GenreModelOutput(Guid id, string name, bool isActive, DateTime createdAt, List<Guid> categories)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }
}
