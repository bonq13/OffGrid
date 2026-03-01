using OffGrid.Models.Enums;

namespace OffGrid.Models;

public abstract class Item
{
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = string.Empty;
    public double WeightGrams { get; set; }
    public ItemCategory Category { get; set; }
    public ItemCondition Condition { get; set; } = ItemCondition.Good;
    
    public DateTime AddedDate { get; set; } = DateTime.Now;

    public string? Notes { get; set; }

    protected Item(string name, double weightGrams)
    {
        Name = name;
        WeightGrams = weightGrams;
    }
    
    public abstract string GetSummary();
}