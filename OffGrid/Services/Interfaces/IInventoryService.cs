using OffGrid.Models;
using OffGrid.Models.Enums;

namespace OffGrid.Services.Interfaces;

public interface IInventoryService
{
    void AddItem(Item item);
    void RemoveItem(Guid id);
    Item GetById(Guid id);
    List<Item> GetAll();
    List<Item> GetByCategory(ItemCategory category);
    List<Item> GetByCondition(ItemCondition condition);
    Item UpdateItem(Item item);
    List<Item> GetItemsNeedingService();
}