﻿
using JG.Flix.Catalog.Domain.SeedWork;
using JG.Flix.Catalog.Domain.Validation;

namespace JG.Flix.Catalog.Domain.Entity;
public class Genre: AggregateRoot
{    
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Genre(string name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void Update(string name) => Name = name;

    private void Validate() => DomainValidation.NotNullOrEmpty(Name, nameof(Name));
}
