namespace OffGrid.Models;

public class Backpack
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public double MaxWeightKg { get; set; }
    
    public List<Item> Items { get; set; } = new List<Item>();
    
    public double CurrentWeightKg => Items.Sum(item => item.WeightGrams / 1000);
    
    public double RemainingWeightKg => MaxWeightKg - CurrentWeightKg;
    public bool IsOverweight => CurrentWeightKg > MaxWeightKg;
    
    public Backpack(string name, double maxWeightKg)
    {
        Name = name;
        MaxWeightKg = maxWeightKg;
    }
}