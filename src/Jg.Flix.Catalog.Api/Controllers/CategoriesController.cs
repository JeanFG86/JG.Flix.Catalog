using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jg.Flix.Catalog.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);
        return CreatedAtAction(nameof(Create), new { output.Id }, output);
    }
}
