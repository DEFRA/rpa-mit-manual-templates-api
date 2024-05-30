using Rpa.Mit.Manual.Templates.Api.Api.Models;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Exceptions;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using Asp.Versioning;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace Rpa.Mit.Manual.Templates.Api.Api.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;
    public DemoController(IItemService itemService, IMapper mapper, ILogger<DemoController> logger)
    {
        _itemService = itemService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// GET method
    /// </summary>
    /// <returns>ActionResult</returns>
    [HttpGet("", Name = "GetAllItems")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Item>))]
    public async Task<IActionResult> GetAllItemsAsync()
    {
        _logger.LogInformation("GET method on Demo controller to getAll");
        var result = await _itemService.GetAllItemsAsync();
        return Ok(result);
    }

    /// <summary>
    /// GET by Id method
    /// </summary>
    /// <param name="id">Id of the object to be retrieved</param>
    /// <returns>ActionResult</returns>
    [HttpGet("{id}", Name = "GetItemById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("GET method on Demo controller to getById");
            var result = await _itemService.GetItemByIdAsync(id);
            return Ok(result);
        }
        catch (ItemNotFoundException ex)
        {
            _logger.LogError(ex, "Retrieving item threw exception: {Message}", ex.Message);
            return NotFound();
        }
    }

    /// <summary>
    /// POST to create a new item
    /// </summary>
    /// <param name="item">item to be created</param>
    /// <returns>Item created</returns>
    [HttpPost("", Name = "CreateItem")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Item))]
    public async Task<IActionResult> CreateItemAsync([FromBody] ItemRequest item)
    {
        _logger.LogInformation("POST method on Demo controller to create");
        var itemEntity = _mapper.Map<Item>(item);
        var result = await _itemService.CreateItemAsync(itemEntity);
        return new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    /// <summary>
    /// PATCH to update an existing item
    /// </summary>
    /// <param name="item">item to be created</param>
    /// <returns>Item created</returns>
    [HttpPatch("", Name = "UpdateItem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItemAsync([FromBody] ItemRequest item)
    {
        try
        {
            _logger.LogInformation("PATCH method on Demo controller to update");
            var itemEntity = _mapper.Map<Item>(item);
            var result = await _itemService.UpdateItemAsync(itemEntity);
            return Ok(result);
        }
        catch (ItemNotFoundException ex)
        {
            _logger.LogError(ex, "Updating item threw exception: {Message}", ex.Message);
            return NotFound();
        }
    }

    /// <summary>
    /// DELETE to delete an existing item
    /// </summary>
    /// <param name="id">Id of the item to be deleted</param>
    [HttpDelete("{id}", Name = "DeleteItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItemAsync(int id)
    {
        try
        {
            _logger.LogInformation("PATCH method on Demo controller to update");
            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
        catch (ItemNotFoundException ex)
        {
            _logger.LogError(ex, "Deleting item threw exception: {Message}", ex.Message);
            return NotFound();
        }
    }
}

