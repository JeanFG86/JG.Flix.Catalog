using JG.Flix.Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Flix.Catalog.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if(target == null) 
            throw new EntityValidationException($"{fieldName} should not be null");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target)) 
            throw new EntityValidationException($"{fieldName} should not be null or empty");
    }

    public static void MinLength(string? target, int minLength, string fieldName)
    {
        if(target.Length < minLength)
        {
            throw new EntityValidationException($"{fieldName} should not be less than {minLength}");
        }
    }
}
