using OffGrid.Models;
using OffGrid.Models.Enums;

namespace OffGrid.Services.Interfaces;

public interface IExpeditionService
{
    void AddExpedition(ExpeditionPlan expedition);
    void RemoveExpedition(Guid id);
    ExpeditionPlan GetByName(string name);
    ExpeditionPlan GetByStartDate(DateTime startDate);
    List<ExpeditionPlan> GetAll();
    ExpeditionPlan AddExpeditionNote(Guid expeditionId, string note);
    ExpeditionPlan AddExpeditionRoutePoint(Guid expeditionId, RoutePoint routePoint);
    ExpeditionPlan RemoveExpeditionRoutePoint(Guid expeditionId, Guid routePointId);
    void SetParticipantsCount(Guid expeditionId, int count);
    (ExpeditionPlan plan, List<ItemCategory> missing5C) CreateExpedition(string name, DateTime startDate, DateTime endDate, Backpack gear);
    
}