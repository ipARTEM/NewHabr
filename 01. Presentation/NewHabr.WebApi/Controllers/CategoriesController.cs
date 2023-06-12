﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a specific category
    /// </summary>
    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetById([FromRoute] int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryService
            .GetByIdAsync(categoryId, cancellationToken);
        return Ok(category);
    }

    /// <summary>
    /// Retrieves all categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.GetAllAsync(cancellationToken));
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { categoryId = category.Id }, category);
    }

    /// <summary>
    /// Updates a specific category
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(
        [FromRoute, Range(1, int.MaxValue)] int id,
        [FromBody] CategoryUpdateRequest categoryToUpdate,
        CancellationToken cancellationToken)
    {
        await _categoryService.UpdateAsync(id, categoryToUpdate, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Deletes a specific category
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteById([FromRoute, Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteByIdAsync(id, cancellationToken);
        return Ok();
    }
}
