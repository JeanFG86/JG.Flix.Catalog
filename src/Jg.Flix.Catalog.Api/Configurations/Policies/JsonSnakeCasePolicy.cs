using Jg.Flix.Catalog.Api.Extensions.String;
using System.Text.Json;

namespace Jg.Flix.Catalog.Api.Configurations.Policies;

class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToSnakeCase();
}
