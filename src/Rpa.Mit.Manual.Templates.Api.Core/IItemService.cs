using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
public interface IItemService
{
    /// <summary>
    /// Get all items
    /// </summary>
    /// <returns>List of all items</returns>
    public Task<List<Item>> GetAllItemsAsync();

    /// <summary>
    /// Get item by id property
    /// </summary>
    /// <param name="id">Id of the item</param>
    /// <returns>Item if present</returns>
    public Task<Item> GetItemByIdAsync(int id);

    /// <summary>
    /// Create new item
    /// </summary>
    /// <param name="item">Item object to be created</param>
    /// <returns>Created Item object</returns>
    public Task<Item> CreateItemAsync(Item item);

    /// <summary>
    /// Update existing item
    /// </summary>
    /// <param name="item">Item object to be updated</param>
    /// <returns>Updated Item object</returns>
    public Task<Item> UpdateItemAsync(Item item);

    /// <summary>
    /// Delete existing item
    /// </summary>
    /// <param name="id">Id of the item to be deleted</param>
    /// <returns>Delted item</returns>
    public Task<Item> DeleteItemAsync(int id);

}
