using OffGrid.Services;

namespace OffGrid;

class Program
{
    static void Main(string[] args)
    {
        BackpackService backpackService = new();
        InventoryService inventoryService = new();
        AutonomyCalculator autonomyCalculator = new();
        SkillService skillService = new();
        ExpeditionService expeditionService = new ExpeditionService(backpackService);
        ConsoleMenu consoleMenu = new ConsoleMenu(backpackService, inventoryService, autonomyCalculator, skillService, expeditionService);
        consoleMenu.Run();
        
    }
}