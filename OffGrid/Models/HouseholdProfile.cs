namespace OffGrid.Models;

public class HouseholdProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<HouseholdMember> Members { get; set; } = new List<HouseholdMember>();
    
    public double DailyCaloriesNeeded => Members.Sum(member => member.DailyCaloriesNeeded);
    public double DailyWaterLitersNeeded => Members.Sum(member => member.DailyWaterLitersNeeded);
    
    public string BaseLocation { get; set; } = string.Empty;
}