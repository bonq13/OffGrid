using OffGrid.Models;
using OffGrid.Models.Enums;

namespace OffGrid.Services.Interfaces;

public interface IBackpackService
{
    void PutIntoBackpack(Item item, Backpack backpack);
    void RemoveFromBackpack(Guid id, Backpack backpack);
    bool IsOverweight(Backpack backpack);
    double GetOverweightKg(Backpack backpack);
    double GetBackpackWeight(Backpack backpack);
    double GetBackpackMaxWeight(Backpack backpack);
    void EmptyBackpack(Backpack backpack);
    List<ItemCategory> Check5CRule(Backpack backpack);
}