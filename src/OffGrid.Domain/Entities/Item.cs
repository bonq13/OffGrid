using System.Diagnostics.CodeAnalysis;
using OffGrid.Domain.Enums;

namespace OffGrid.Domain.Entities;

public abstract class Item
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public double WeightGrams { get; private set; }
    public ItemCategory Category { get; private set; }
    public ItemCondition Condition { get; private set; } = ItemCondition.Good;
    public DateOnly AddedOn { get; private set; }
    
    protected Item(string name, double weightGrams, ItemCategory category, DateOnly addedOn)
    {
        SetName(name);
        SetWeightGrams(weightGrams);
        Category = category;
        AddedOn = addedOn;
    }
    
    public void UpdateItem(string name, double weightGrams, ItemCondition condition)
    {
        SetName(name);
        SetWeightGrams(weightGrams);
        UpdateCondition(condition);
    }
    
    public void UpdateCondition(ItemCondition condition)
    {
        if (!Enum.IsDefined(condition))
        {
            throw new ArgumentOutOfRangeException(nameof(condition), condition, "Invalid item condition");
        }
        Condition = condition;
    }
    
    [MemberNotNull(nameof(Name))]
    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name); 
        Name = name;
    }
    
    private void SetWeightGrams(double weightGrams)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(weightGrams, 0);
        WeightGrams = weightGrams;
    }
}