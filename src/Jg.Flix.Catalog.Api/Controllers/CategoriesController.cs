﻿using JG.Flix.Catalog.Application.UseCases.Category.Common;
using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;
using JG.Flix.Catalog.Application.UseCases.Category.DeleteCategory;
using JG.Flix.Catalog.Application.UseCases.Category.GetCategory;
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(new GetCategoryInput(id), cancellationToken);
        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryInput(id), cancellationToken);
        return NoContent();
    }
}