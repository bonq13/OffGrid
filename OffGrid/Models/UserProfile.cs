using OffGrid.Models.Enums;
using OffGrid.Services;

namespace OffGrid.Models;

public class UserProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int AgeYears { get; set; }
    public double WeightKg { get; set; }
    public ActivityLevel ActivityLevel { get; set; } = ActivityLevel.Moderate;
    
    public double DailyCaloriesNeeded => NutritionCalculator.CalculateCalories(WeightKg, AgeYears, ActivityLevel);
    public double DailyWaterLitersNeeded => NutritionCalculator.CalculateWater(WeightKg, AgeYears, ActivityLevel);

    public UserProfile(string name, int ageYears, double weightKg)
    {
        Name = name;
        AgeYears = ageYears;
        WeightKg = weightKg;
    }
}