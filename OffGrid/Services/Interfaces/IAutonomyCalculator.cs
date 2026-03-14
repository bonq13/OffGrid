using OffGrid.Models;

namespace OffGrid.Services.Interfaces;

public interface IAutonomyCalculator
{
    int CalculateFoodAutonomyDays(List<FoodItem> foodItems, HouseholdProfile householdProfile);
    int CalculateWaterAutonomyDays(List<WaterItem> waterItems, HouseholdProfile householdProfile);
    List<Alert> GenerateAlerts(List<FoodItem> foodItems, List<WaterItem> waterItems, HouseholdProfile householdProfile);
    
}