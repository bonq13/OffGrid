using OffGrid.Data;
using OffGrid.Models;
using OffGrid.Services;
using QuestPDF;
using QuestPDF.Infrastructure;

namespace OffGrid;

class Program
{
    static void Main(string[] args)
    {
        Settings.License = LicenseType.Community;
        
        var itemRepo = new JsonRepository<Item>();
        var skillRepo = new JsonRepository<Skill>();
        
        var loadedItems = itemRepo.Load("inventory");
        var loadedSkills = skillRepo.Load("skills");
        
        var inventoryService = new InventoryService(loadedItems);
        var skillService = new SkillService(loadedSkills);

        
        var backpackService = new BackpackService();
        var autonomyCalculator = new AutonomyCalculator();
        var expeditionService = new ExpeditionService(backpackService);

        
        var consoleMenu = new ConsoleMenu(
            backpackService,
            inventoryService,
            autonomyCalculator,
            skillService,
            expeditionService);

        consoleMenu.Run();
        
        try
        {
            itemRepo.Save(inventoryService.GetAll(), "inventory");
            skillRepo.Save(skillService.GetAll(), "skills");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Błąd zapisu: {e.Message}");
        }
        Console.ReadKey();
    }
}