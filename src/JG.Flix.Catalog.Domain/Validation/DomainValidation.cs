using JG.Flix.Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Flix.Catalog.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object target, string fieldName)
    {
        if(target == null) throw new EntityValidationException($"{fieldName} should not be null");
    }
}
