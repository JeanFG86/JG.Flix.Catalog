using Jg.Flix.Catalog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
builder.Services
    .AddUseCases()
    .AddAndConfigureControllers();

app.UseDocumentation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
