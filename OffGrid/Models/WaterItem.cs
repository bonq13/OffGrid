using OffGrid.Models.Enums;

namespace OffGrid.Models;

public class WaterItem : Item
{
    public double VolumeLiters { get; set; }
    public bool IsPurified { get; set; }
    
    public string PurityStatus => IsPurified ? "Zdatna do picia" : "Wymaga uzdatnienia";

    public WaterItem(string name, double volumeLiters,  bool isPurified = true)
        : base(name, volumeLiters * 1000)
    {
        VolumeLiters = volumeLiters;
        IsPurified = isPurified;
        Category = ItemCategory.Water;
    }

    public override string GetSummary()
    {
        return $"{Name} | {VolumeLiters:F1}L | {PurityStatus}";
    }
}