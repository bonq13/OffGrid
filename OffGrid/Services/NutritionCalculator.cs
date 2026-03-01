using OffGrid.Models.Enums;

namespace OffGrid.Services;

public static class NutritionCalculator
{

    private static double GetPalFactor(ActivityLevel activityLevel)
    {
        return activityLevel switch
        {
            ActivityLevel.Resting => 1.2,
            ActivityLevel.Light => 1.5,
            ActivityLevel.Moderate => 1.75,
            ActivityLevel.Heavy => 2.0,
            _ => 1.75
        };
    }
    public static double CalculateCalories(double weight, int age, ActivityLevel activityLevel)
    {
        
        double basalMetabolicRate = (10 * weight) + (6.25 * 170) - (5 * age) + 5;
        
        double dailyCaloriesNeeded = basalMetabolicRate * GetPalFactor(activityLevel);
        
        return dailyCaloriesNeeded;
    }

    public static double CalculateWater(double weight, int age, ActivityLevel activityLevel)
    {
        double dailyWaterNeeded = (weight * 35 / 1000) * (GetPalFactor(activityLevel) / 1.75);
        
        return dailyWaterNeeded;
    }
}