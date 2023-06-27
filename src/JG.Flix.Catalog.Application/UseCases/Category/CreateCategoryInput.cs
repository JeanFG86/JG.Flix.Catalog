namespace JG.Flix.Catalog.Application.UseCases.Category;
public class CreateCategoryInput
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
