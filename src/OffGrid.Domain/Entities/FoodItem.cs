using OffGrid.Domain.Enums;

namespace OffGrid.Domain.Entities;

public class FoodItem : Item
{
    private const double MaxCaloriesPer100g = 900; //pure fat 900 kcal / 100 g
    public double CaloriesPer100g { get; private set; }
    public DateOnly ExpiryDate { get; private set; }
    public double TotalCalories => (CaloriesPer100g / 100 ) * WeightGrams;
    
    public FoodItem(string name, double weightGrams, double caloriesPer100g, DateOnly addedOn, DateOnly expiryDate)
        : base(name, weightGrams, ItemCategory.Food, addedOn)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(caloriesPer100g, MaxCaloriesPer100g);
        ArgumentOutOfRangeException.ThrowIfLessThan(caloriesPer100g, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(expiryDate, addedOn);
        CaloriesPer100g = caloriesPer100g;
        ExpiryDate = expiryDate;
    }
    
    public int DaysUntilExpiry(DateOnly today)
    {
        return ExpiryDate.DayNumber - today.DayNumber;
    }

    public bool IsExpiredOn(DateOnly day)
    {
        return DaysUntilExpiry(day) < 0;
    }
    
}