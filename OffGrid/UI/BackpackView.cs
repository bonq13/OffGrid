using OffGrid.Models;
using OffGrid.Services.Interfaces;

namespace OffGrid.UI;

public class BackpackView
{
    private readonly IBackpackService _backpackService;
    private readonly IInventoryService _inventoryService;
    
    public BackpackView(IBackpackService backpackService, IInventoryService inventoryService)
    {
        _backpackService = backpackService;
        _inventoryService = inventoryService;
    }
    
    private readonly List<Backpack> _backpacks = new List<Backpack>();
    
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Plecak ===");
            Console.WriteLine("1. Dodaj nowy plecak");
            Console.WriteLine("2. Usuń plecak");
            Console.WriteLine("3. Wybierz plecak");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję: ");
            
            var input = Console.ReadLine();

            switch (input)
            {
                case "1": AddBackpack(); break;
                case "2": RemoveBackpack(); break;
                case "3": ChooseBackpack(); break;
                case "0": return;
                default:
                    Console.WriteLine("Nieznana opcja, naciśnij dowolny klawisz...");
                    Console.ReadKey();
                    break;
            }
            
            
        }
    }

    private Backpack? SelectedBackpack()
    {
        if (_backpacks.Count == 0)
        {
            Console.WriteLine("Brak plecaków. Najpierw dodaj plecak.");
            Console.ReadKey();
            return null;
        }

        for (int i = 0; i < _backpacks.Count; i++)
            Console.WriteLine($"{i + 1}. {_backpacks[i].Name} | " +
                              $"{_backpacks[i].CurrentWeightKg:F1}/{_backpacks[i].MaxWeightKg}kg");

        Console.Write("Wybierz numer plecaka: ");
        if (!int.TryParse(Console.ReadLine(), out int choice)
            || choice < 1 || choice > _backpacks.Count)
        {
            Console.WriteLine("Nieprawidłowy wybór.");
            Console.ReadKey();
            return null;
        }

        return _backpacks[choice - 1];
    }
    

    private void ChooseBackpack()
    {
        var backpack = SelectedBackpack();
        if (backpack == null) return;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {backpack.Name} ===");
            Console.WriteLine("1. Szczegóły i zawartość");
            Console.WriteLine("2. Dodaj przedmiot");
            Console.WriteLine("3. Usuń przedmiot");
            Console.WriteLine("4. Opróżnij plecak");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1": ShowBackpackDetails(backpack); break;
                case "2": AddItemToBackpack(backpack); break;
                case "3": RemoveItemFromBackpack(backpack); break;
                case "4": UnpackBackpack(backpack); break;
                case "0": return;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    private void AddBackpack()
    {
        Console.WriteLine("Podaj nazwę plecaka: ");
        var backpackName = Console.ReadLine();
        
        Console.WriteLine("Podaj maksymalną wagę plecaka (w kilogramach): ");
        if (!double.TryParse(Console.ReadLine(), out var maxWeight))
        {
            Console.WriteLine("Niepoprawna wartość wagi...");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Podaj wagę samego plecaka (w gramach): ");
        if (!double.TryParse(Console.ReadLine(), out var selfWeight))
        {
            Console.WriteLine("Niepoprawna wartość wagi...");
            Console.ReadKey();
            return;
        }
        
        Backpack backpack = new Backpack(backpackName, maxWeight, selfWeight);
        _backpacks.Add(backpack);
        Console.WriteLine("Plecak dodany.");
        Console.ReadKey();
    }
    
    private void RemoveBackpack()
    {
        var backpack = SelectedBackpack();
        if (backpack == null) return;

        _backpacks.Remove(backpack);
        Console.WriteLine("Plecak usunięty");
        Console.ReadKey();
    }

    private void AddItemToBackpack(Backpack backpack)
    {
        var items = _inventoryService.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("Inwentarz jest pusty. Najpierw dodaj przedmioty.");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("Wybierz przedmiot do spakowania: ");

        foreach (var item in items)
        {
            Console.WriteLine($"{items.IndexOf(item) + 1}. {item.Name} | {item.Category.ToPolish()} | {item.WeightGrams}");
        }
        
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > items.Count)
        {
            Console.WriteLine("Niepoprawny indeks przedmiotu...");
            Console.ReadKey();
            return;
        }

        var itemToPack = items[choice - 1];
        
        try
        {
            _backpackService.PutIntoBackpack(itemToPack, backpack);
            Console.WriteLine($"Dodano {itemToPack.Name} do plecaka {backpack.Name}.");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.ReadKey();
            return;
        }

        var check5C = _backpackService.Check5CRule(backpack);

        if (check5C.Count != 0)
        {
            foreach (var lackingItem in check5C)
            {
                Console.WriteLine($"W twoim plecaku brakuje jeszcze: {lackingItem.ToPolish()}");
            }
        }
        
        Console.ReadKey();
    }

    private void RemoveItemFromBackpack(Backpack backpack)
    {
        var items = backpack.Items;
        if (items.Count == 0)
        {
            Console.WriteLine("Plecak jest pusty.");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine($"Wybierz przedmiot, który chcesz usunąć z plecaka {backpack.Name}: ");
        foreach (var item in items)
        {
            Console.WriteLine($"{items.IndexOf(item) + 1}. {item.Name} | {item.Category.ToPolish()} | {item.WeightGrams}");
        }
        
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > items.Count)
        {
            Console.WriteLine("Niepoprawny indeks przedmiotu...");
            Console.ReadKey();
            return;
        }
        var itemToRemove = items[choice - 1].Id;

        try
        {
            _backpackService.RemoveFromBackpack(itemToRemove, backpack);
            Console.WriteLine("Przedmiot usunięty");
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadKey();
    }

    private void UnpackBackpack(Backpack backpack)
    {
        _backpackService.EmptyBackpack(backpack);
        Console.WriteLine("Plecak rozpakowany.");
        Console.ReadKey();
    }
    
    private void ShowBackpackDetails(Backpack backpack)
    {
        var items = backpack.Items;
        
        Console.WriteLine($"Nazwa: {backpack.Name}");
        Console.WriteLine($"Maksymalna waga: {backpack.MaxWeightKg}kg");
        Console.WriteLine($"Waga samego plecaka: {backpack.Weight}g");
        if (backpack.IsOverweight)
        {
            Console.WriteLine($"Przekroczono limit wagi - {backpack.CurrentWeightKg:F1}kg/{backpack.MaxWeightKg:F1}kg");
        }
        else
        {
            Console.WriteLine($"Aktualna waga: {backpack.CurrentWeightKg:F1}kg - w normie");
        }

        if (items.Count == 0)
        {
            Console.WriteLine("Plecak jest pusty");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.GetSummary());
            }
        }
        
        Console.ReadKey();
    }
}