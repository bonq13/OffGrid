using OffGrid.Models;
using OffGrid.Services.Interfaces;
using OffGrid.UI;

namespace OffGrid;

public class ConsoleMenu
{
    private readonly IBackpackService _backpackService;
    private readonly IInventoryService _inventoryService;
    private readonly IAutonomyCalculator _autonomyCalculator;
    private readonly ISkillService _skillService;
    private readonly IExpeditionService _expeditionService;
    private readonly InventoryView _inventoryView;
    private readonly BackpackView _backpackView;
    private readonly ExpeditionView _expeditionView;
    private readonly AutonomyView _autonomyView;
    private readonly SkillView _skillView;
    public ConsoleMenu(IBackpackService backpackService, IInventoryService inventoryService,
        IAutonomyCalculator autonomyCalculator, ISkillService skillService, IExpeditionService expeditionService)
    {
        _backpackService = backpackService;
        _inventoryService = inventoryService;
        _autonomyCalculator = autonomyCalculator;
        _skillService = skillService;
        _expeditionService = expeditionService;
        _inventoryView = new InventoryView(_inventoryService);
        var sharedBackpacks = new List<Backpack>();
        _backpackView = new BackpackView(backpackService, inventoryService, sharedBackpacks);
        _expeditionView = new ExpeditionView(backpackService, expeditionService, sharedBackpacks);
        _autonomyView = new AutonomyView(_inventoryService, _autonomyCalculator);
        _skillView = new SkillView(_skillService);
    }
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== OffGrid ===");
            Console.WriteLine("1. Inwentarz");
            Console.WriteLine("2. Plecak");
            Console.WriteLine("3. Autonomia");
            Console.WriteLine("4. Umiejętności");
            Console.WriteLine("5. Wyprawy");
            Console.WriteLine("0. Wyjście");
            Console.Write("\nWybierz opcję: ");
            
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _inventoryView.Run();
                    break;
                case "2":
                    _backpackView.Run();
                    break;
                case "3":
                    _autonomyView.Run();
                    break;
                case "4":
                    _skillView.Run();
                    break;
                case "5":
                    _expeditionView.Run();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Nieznana opcja, naciśnij dowolny klawisz...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}