using OffGrid.Models.Enums;

namespace OffGrid.Models;

public class Alert
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public AlertCategory Category {get; set;}
    public AlertPriority Priority {get; set;}
    public string Message {get; set;}
    public DateTime Date {get; set;}
    
    public Alert(AlertCategory category, AlertPriority priority, string message)
    {
        Category = category;
        Priority = priority;
        Message = message;
        Date = DateTime.Now;
    }
}