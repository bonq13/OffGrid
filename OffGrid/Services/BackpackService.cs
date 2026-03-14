using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.Services;

public class BackpackService : IBackpackService
{
    public void PutIntoBackpack(Item item, Backpack backpack)
    {
        if(item == null) throw new ArgumentNullException(nameof(item));
        if (backpack.IsOverweight || item.WeightGrams > backpack.RemainingWeightKg * 1000)
        {
            throw new InvalidOperationException("Backpack is overweight!");
        }
        
        backpack.Items.Add(item);
    }
    
    public void RemoveFromBackpack(Guid id, Backpack backpack)
    {
        var item = backpack.Items.FirstOrDefault(item => item.Id == id);
        if (item == null)
        {
            throw new KeyNotFoundException($"Przedmiot o id: {id} nie istnieje!");
        }
        backpack.Items.Remove(item);
    }
    
    public bool IsOverweight(Backpack backpack)
    {
        return backpack.IsOverweight;
    }
    
    public double GetOverweightKg(Backpack backpack)
    {
        return Math.Max(0, backpack.CurrentWeightKg - backpack.MaxWeightKg);
    }
    
    public double GetBackpackWeight(Backpack backpack)
    {
        return backpack.CurrentWeightKg;
    }
    
    public double GetBackpackMaxWeight(Backpack backpack)
    {
        return backpack.MaxWeightKg;
    }
    
    public void EmptyBackpack(Backpack backpack)
    {
        backpack.Items.Clear();
    }
    
    public List<ItemCategory> Check5CRule(Backpack backpack)
    {
        var required5CCategories = new List<ItemCategory>
        {
            ItemCategory.CuttingTool,
            ItemCategory.FireStarter,
            ItemCategory.Shelter,
            ItemCategory.Cordage,
            ItemCategory.WaterContainer
        };

        return required5CCategories.Where(category => !backpack.Items.Any(item => item.Category == category)).ToList();

    }
}