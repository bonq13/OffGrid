using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.Services;

public class InventoryService : IInventoryService
{
    private readonly List<Item> _items = new List<Item>();

    public InventoryService() { }
    public InventoryService(List<Item> items)
    {
        _items = items;
    }

    public void AddItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Add(item);
    }

    public void RemoveItem(Guid id)
    {
        var item = _items.FirstOrDefault(item => item.Id == id);
        if (item == null)
        {
            throw new KeyNotFoundException($"Przedmiot o id: {id} nie istnieje!");
        }
        _items.Remove(item);
    }
    
    public Item GetById(Guid id)
    {
        var item = _items.FirstOrDefault(item => item.Id == id);
        if (item == null)
        {
            throw new KeyNotFoundException($"Przedmiot o id: {id} nie istnieje!");
        }
        return item;
    }
    
    public List<Item> GetAll()
    {
        return _items.ToList();
    }

    public List<Item> GetByCategory(ItemCategory category)
    {
        var items = _items.Where(item => item.Category == category).ToList();
        return items;
    }

    public List<Item> GetByCondition(ItemCondition condition)
    {
        var items = _items.Where(item => item.Condition == condition).ToList();
        return items;
    }

    public Item UpdateItem(Item item)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem == null)
        {
            throw new KeyNotFoundException($"Przedmiot o id: {item.Id} nie istnieje!");
        }
        _items.Remove(existingItem);
        _items.Add(item);
        return item;
    }
    
    public List<Item> GetItemsNeedingService()
    {
        return _items.Where(item => item.Condition == ItemCondition.NeedsService 
                                    || item is GearItem gear && gear.NeedsServiceSoon).ToList();
    }
}