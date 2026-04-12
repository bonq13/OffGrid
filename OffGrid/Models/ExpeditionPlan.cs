namespace OffGrid.Models;

public class ExpeditionPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<RoutePoint> RoutePoints { get; set; } = new List<RoutePoint>();
    public int DurationDays => (int) (EndDate - StartDate).TotalDays;
    public int EstimatedKilometers {get; set;}
    public int ExpeditionParticipants {get; set;}
    public double EstimatedCost {get; set;}
    public string? Notes { get; set; }
    public Backpack? Gear { get; set; }
    
    public ExpeditionPlan(string name, DateTime startDate, DateTime endDate, Backpack gear)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Gear = gear;
    }
}