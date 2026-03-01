using OffGrid.Models.Enums;

namespace OffGrid.Models;

public class FoodItem : Item
{
    public double CaloriesPer100g { get; set; }
    public double QuantityGrams { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    public double TotalCalories => (CaloriesPer100g / 100 ) * QuantityGrams;
    
    public int DaysUntilExpiry => (ExpiryDate - DateTime.Now).Days;

    public FoodItem(string name, double quantityGrams, double caloriesPer100g, DateTime expiryDate)
        : base(name, quantityGrams)
    {
        QuantityGrams = quantityGrams;
        CaloriesPer100g = caloriesPer100g;
        ExpiryDate = expiryDate;
        Category = ItemCategory.Food;
    }
    
    public override string GetSummary()
    {
        return $"{Name} | {QuantityGrams}g | {TotalCalories:F0}kcal | " +
               $"Wazne do {ExpiryDate:dd-MM-yyyy} | ({DaysUntilExpiry} dni)";
    }
}