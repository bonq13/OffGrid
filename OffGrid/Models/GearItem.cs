using OffGrid.Models.Enums;

namespace OffGrid.Models;

public class GearItem : Item
{
    public DateTime? LastServiceDate { get; set; }
    public double UsageHours { get; set; }
    
    public bool IsPartOf5C { get; set; }
    
    public ItemCategory? Requires5CCategory { get; set; }
    
    public bool NeedsServiceSoon => UsageHours > 50 && Condition != ItemCondition.Damaged;

    public int? DaysSinceService => LastServiceDate.HasValue
        ? (DateTime.Now - LastServiceDate.Value).Days
        : null;

    public GearItem(string name, double weightGrams, bool isPartOf5C = false)
        : base(name, weightGrams)
    {
        IsPartOf5C = isPartOf5C;
        Category = isPartOf5C ? ItemCategory.CuttingTool : ItemCategory.Tools;
    }

    public override string GetSummary()
    {
        var serviceInfo = DaysSinceService.HasValue
            ? $"Serwis: {DaysSinceService} dni temu"
            : "Brak serwisu";
        
        var warningFlag = NeedsServiceSoon ? "| Wymaga przeglądu ⚠️" : string.Empty;
        
        return ($"{Name} | {WeightGrams}g | Stan: {Condition} | " +
                $"{serviceInfo} {warningFlag}");
    }
}