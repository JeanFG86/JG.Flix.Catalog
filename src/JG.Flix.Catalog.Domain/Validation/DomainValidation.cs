﻿using JG.Flix.Catalog.Domain.Exceptions;
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
            throw new EntityValidationException($"{fieldName} should not be empty or null");
    }

    public static void MinLength(string? target, int minLength, string fieldName)
    {
        if(target.Length < minLength)
        {
            throw new EntityValidationException($"{fieldName} should be at least {minLength} characteres long");
        }
    }

    public static void MaxLength(string? target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength)
        {
            throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characteres long");
        }
    }
}
