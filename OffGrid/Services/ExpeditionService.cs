using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.Services;

public class ExpeditionService : IExpeditionService
{
    private readonly IBackpackService _backpackService;
    private readonly List<ExpeditionPlan> _expeditions = new List<ExpeditionPlan>();
    
    public ExpeditionService(IBackpackService backpackService)
    {
        _backpackService = backpackService;
    }
    
    public void AddExpedition(ExpeditionPlan expedition)
    {
        if (expedition == null) throw new ArgumentNullException(nameof(expedition));
        _expeditions.Add(expedition);
    }

    public void RemoveExpedition(Guid id)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Id == id);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Wyprawa o id: {id} nie istnieje!");
        }
        _expeditions.Remove(expedition);
    }
    
    public ExpeditionPlan GetById(Guid id)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Id == id);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Wyprawa o id: {id} nie istnieje!");
        }
        return expedition;
    }
    
    public ExpeditionPlan GetByName(string name)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Name == name);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Wyprawa o nazwie: {name} nie istnieje!");
        }
        return expedition;
    }
    
    public ExpeditionPlan GetByStartDate(DateTime startDate)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.StartDate == startDate);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Nie zaplanowano wyprawy na: {startDate}");
        }
        return expedition;
    }
    
    public List<ExpeditionPlan> GetAll()
    {
        return _expeditions.ToList();
    }

    public ExpeditionPlan AddExpeditionNote(Guid expeditionId, string note)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Id == expeditionId);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Wyprawa o id: {expeditionId} nie istnieje!");
        }
        expedition.Notes = note;
        return expedition;
    }
    
    public ExpeditionPlan AddExpeditionRoutePoint(Guid expeditionId, RoutePoint routePoint)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Id == expeditionId);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Wyprawa o id: {expeditionId} nie istnieje!");
        }

        if (routePoint == null)
        {
            throw new ArgumentNullException(nameof(routePoint));
        }
        expedition.RoutePoints.Add(routePoint);
        return expedition;
    }

    public ExpeditionPlan RemoveExpeditionRoutePoint(Guid expeditionId, Guid routePointId)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Id == expeditionId);
        var routePoint = expedition?.RoutePoints.FirstOrDefault(point => point.Id == routePointId);
        if (expedition == null || routePoint == null)
        {
            throw new KeyNotFoundException($"Wyprawa o id: {expeditionId} lub punkcie trasy o id: {routePointId} nie istnieje!");
        }
        expedition.RoutePoints.Remove(routePoint);
        return expedition;
    }

    public void SetParticipantsCount(Guid expeditionId, int count)
    {
        var expedition = _expeditions.FirstOrDefault(expedition => expedition.Id == expeditionId);
        if (expedition == null)
        {
            throw new KeyNotFoundException($"Wyprawa o id: {expeditionId} nie istnieje!");
        }
        expedition.ExpeditionParticipants = count;
    }

    public (ExpeditionPlan plan, List<ItemCategory> missing5C) CreateExpedition(string name, DateTime startDate,
        DateTime endDate, Backpack gear)
    {
        ExpeditionPlan expedition = new ExpeditionPlan(name, startDate, endDate, gear);
        _expeditions.Add(expedition);
        List<ItemCategory> missing5C = _backpackService.Check5CRule(gear);
        return (expedition, missing5C);
    }
}