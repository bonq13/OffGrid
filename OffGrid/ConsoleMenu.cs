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
    public ConsoleMenu(IBackpackService backpackService, IInventoryService inventoryService,
        IAutonomyCalculator autonomyCalculator, ISkillService skillService, IExpeditionService expeditionService)
    {
        _backpackService = backpackService;
        _inventoryService = inventoryService;
        _autonomyCalculator = autonomyCalculator;
        _skillService = skillService;
        _expeditionService = expeditionService;
        _inventoryView = new InventoryView(_inventoryService);
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
                    Console.ReadKey();
                    break;
                case "2":
                    Console.WriteLine("Plecak - dostępny wkrótce");
                    Console.ReadKey();
                    break;
                case "3":
                    Console.WriteLine("Autonomia - dostępna wkrótce");
                    Console.ReadKey();
                    break;
                case "4":
                    Console.WriteLine("Umiejętności - dostępne wkrótce");
                    Console.ReadKey();
                    break;
                case "5":
                    Console.WriteLine("Wyprawy - dostępne wkrótce");
                    Console.ReadKey();
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