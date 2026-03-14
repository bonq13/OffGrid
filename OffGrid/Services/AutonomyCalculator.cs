using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.Services;

public class AutonomyCalculator : IAutonomyCalculator
{
    public int CalculateFoodAutonomyDays(List<FoodItem> foodItems, HouseholdProfile householdProfile)
    {
        double calories = foodItems.Sum(item => item.TotalCalories);
        double dailyCaloriesNeeded = householdProfile.DailyCaloriesNeeded;

        double days = calories / dailyCaloriesNeeded;
        return (int)Math.Floor(days);
    }

    public int CalculateWaterAutonomyDays(List<WaterItem> waterItems, HouseholdProfile householdProfile)
    {
        double savedWaterLiters = waterItems.Sum(waterItem => waterItem.VolumeLiters);
        double dailyWaterNeeded = householdProfile.DailyWaterLitersNeeded;
        
        double days = savedWaterLiters / dailyWaterNeeded;
        return (int)Math.Floor(days);
    }

    public List<Alert> GenerateAlerts(List<FoodItem> foods, List<WaterItem> water, HouseholdProfile household)
    {
        List<Alert> alerts = new List<Alert>();
        
        var foodThresholds = new[]
        {
            (maxDays: 1, priority: AlertPriority.Critical, message: "Zostało jedzenia na 1 dzień!"),
            (maxDays: 3, priority: AlertPriority.High, message: "Jedzenia wystarczy na 3 dni!"),
            (maxDays: 7, priority: AlertPriority.Medium, message: "Jedzenia wystarczy na tydzień!"),
            (maxDays: 14, priority: AlertPriority.Low, message: "Jedzenia wystarczy na 2 tygodnie!"),
        };

        var waterThresholds = new[]
        {
            (maxDays: 1, priority: AlertPriority.Critical, message: "Zostało wody na 1 dzień!"),
            (maxDays: 3, priority: AlertPriority.Critical, message: "Wody wystarczy na 3 dni!"),
            (maxDays: 7, priority: AlertPriority.High, message: "Wody wystarczy na tydzień!"),
            (maxDays: 10, priority: AlertPriority.Medium, message: "Wody wystarczy na 10 dni!"),
            (maxDays: 14, priority: AlertPriority.Low, message: "Wody wystarczy na 2 tygodnie!"),
        };
        
        int foodDays = CalculateFoodAutonomyDays(foods, household);
        var matchedFoodThresholds = foodThresholds.FirstOrDefault(t => foodDays <= t.maxDays);

        if (matchedFoodThresholds != default)
        {
            alerts.Add(new Alert(
                AlertCategory.FoodAlert,
                matchedFoodThresholds.priority,
                matchedFoodThresholds.message));
        }
        
        int waterDays = CalculateWaterAutonomyDays(water, household);
        var matchedWaterThresholds = waterThresholds.FirstOrDefault(t => waterDays <= t.maxDays);

        if (matchedWaterThresholds != default)
        {
            alerts.Add(new Alert(
                AlertCategory.WaterAlert,
                matchedWaterThresholds.priority,
                matchedWaterThresholds.message));
        }
        
        return alerts;
    }
    
}