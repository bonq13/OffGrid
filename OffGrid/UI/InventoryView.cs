using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.UI;

public class InventoryView
{
    private readonly IInventoryService _inventoryService;
    public InventoryView(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Inwentarz ===");
            Console.WriteLine("1. Wyświetl wszystko");
            Console.WriteLine("2. Wyszukaj po ID");
            Console.WriteLine("3. Wyszukaj po kategorii");
            Console.WriteLine("4. Wyszukaj po stanie przedmiotu");
            Console.WriteLine("5. Wyświetl przedmioty wymagające naprawy");
            Console.WriteLine("6. Dodaj przedmiot");
            Console.WriteLine("7. Zaktualizuj przedmiot");
            Console.WriteLine("8. Usuń przedmiot");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję:");
            
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ShowAllItems();
                    Console.ReadKey();
                    break;
                case "2":
                    GetItemById();
                    break;
                case "3":
                    GetItemByCategory();
                    
                    break;
                case "4":
                    GetItemByCondition();
                    break;
                case "5":
                    ShowItemsNeedingService();
                    Console.ReadKey();
                    break;
                case "6":
                    AddItem();
                    break;
                case "7":
                    Console.WriteLine("Edycja przedmiotu");
                    Console.ReadKey();
                    break;
                case "8":
                    RemoveItem();
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
    
    private void ShowAllItems()
    {
        var items = _inventoryService.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("Brak przedmiotów");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.GetSummary());
            }
        }
    }

    private void GetItemById()
    {
        var items = _inventoryService.GetAll();

        if (items.Count == 0)
        {
            Console.WriteLine("Lista jest pusta");
        }
        else
        {
            foreach (var item in items)
                Console.WriteLine($"{items.IndexOf(item) + 1}. {item.Name}");
            Console.WriteLine("Wybierz numer przedmiotu:");
            if (!int.TryParse(Console.ReadLine(), out int itemNumber))
            {
                Console.WriteLine("Nieprawidłowa wartość. Naciśnij klawisz...");
                Console.ReadKey();
                return;
            }

            if (itemNumber < 1 || itemNumber > items.Count)
            {
                Console.WriteLine("ID spoza zakresu");
                return;
            }
            var chosenItem = items[itemNumber - 1];
            Console.WriteLine(chosenItem.GetSummary());
            Console.ReadKey();
        }
    }

    private void GetItemByCategory()
    {
        var items = _inventoryService.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("Lista jest pusta");
            return;
        }
        var categories = Enum.GetValues(typeof(ItemCategory)).Cast<ItemCategory>().ToList();
        Console.WriteLine("Wybierz kategorię:");
        for (int i = 0; i < categories.Count; i++)
            Console.WriteLine($"{i + 1}. {categories[i].ToPolish()} ({items.Count(item => item.Category == categories[i])})");
        
        if (!int.TryParse(Console.ReadLine(), out int categoryNumber) 
            || categoryNumber < 1 || categoryNumber > categories.Count)
        {
            Console.WriteLine("Nieprawidłowy wybór.");
            Console.ReadKey();
            return;
        }
        var chosenCategory = categories[categoryNumber - 1];
        
        var itemsByCategory = items.Where(item => item.Category == chosenCategory).ToList();

        foreach (var item in itemsByCategory)
        {
            Console.WriteLine(item.GetSummary());
        }
        Console.ReadKey();

    }

    private void GetItemByCondition()
    {
        var items = _inventoryService.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("Lista jest pusta");
            return;
        }
        var conditions = Enum.GetValues(typeof(ItemCondition)).Cast<ItemCondition>().ToList();
        Console.WriteLine("Wybierz stan przedmiotu:");
        for (int i = 0; i < conditions.Count; i++)
            Console.WriteLine($"{i + 1}. {conditions[i].ToPolish()} ({items.Count(item => item.Condition == conditions[i])})");

        if (!int.TryParse(Console.ReadLine(), out int conditionNumber)
            || conditionNumber < 1 || conditionNumber > conditions.Count)
        {
            Console.WriteLine("Nieprawidłowy wybór.");
            Console.ReadKey();
            return;
        }
        var chosenCondition = conditions[conditionNumber - 1];
        var itemsByCondition = items.Where(item => item.Condition == chosenCondition).ToList();
        
        foreach (var item in itemsByCondition)
        {
            Console.WriteLine(item.GetSummary());
        }
        Console.ReadKey();
    }

    private void AddItem()
    {
        Console.WriteLine("Jakiego typu przedmiot chcesz dodać?");
        Console.WriteLine("1. Jedzenie");
        Console.WriteLine("2. Woda");
        Console.WriteLine("3. Sprzęt");
        Console.Write("\nWybierz typ przedmiotu: ");
        var itemType = Console.ReadLine();
        Console.WriteLine("Podaj nazwę przedmiotu:");
        var name = Console.ReadLine();
        Console.WriteLine("Podaj wagę przedmiotu");
        var weight = Console.ReadLine();
        if (!double.TryParse(weight, out var weightInGrams))
        {
            Console.WriteLine("Nieprawidłowa waga. Naciśnij klawisz...");
            Console.ReadKey();
            return;
        }
        switch (itemType)
        {
            case "1":
                Console.WriteLine("Wybrałeś jedzenie.");
                Console.WriteLine("Podaj jego kaloryczność na 100g:");
                if (!double.TryParse(Console.ReadLine(), out var calories))
                {
                    Console.WriteLine("Nieprawidłowa wartość. Naciśnij klawisz...");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Podaj datę przydatnośći");
                if (!DateTime.TryParse(Console.ReadLine(), out var dateOfUse))
                {
                    Console.WriteLine("Nieprawidłowo wprowadzona data. Naciśnij klawisz... ");
                    Console.ReadKey();
                    return;
                }
                FoodItem foodItem = new FoodItem(name, weightInGrams, calories, dateOfUse);
                _inventoryService.AddItem(foodItem);
                Console.WriteLine($"Dodano: {foodItem.GetSummary()}");
                Console.ReadKey();
                break;
            case "2":
                Console.WriteLine("Wybrałeś wodę.");
                Console.WriteLine("Podaj jej objętość w litrach");
                if (!double.TryParse(Console.ReadLine(), out var volumeLiters))
                {
                    Console.WriteLine("Błędnie wprowadzona objętość wody. Naciśnij klawisz...");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Czy woda jest zdatna do picia? (1) Tak | (Dowolny klawisz) Wymaga uzdatnienia");
                var choice = Console.ReadLine();
                bool isSuitableForDrinking = choice == "1";
                WaterItem waterItem = new WaterItem(name, volumeLiters, isSuitableForDrinking);
                _inventoryService.AddItem(waterItem);
                Console.WriteLine($"Dodano: {waterItem.GetSummary()}");
                Console.ReadKey();
                break;
            case "3":
                Console.WriteLine("Wybrałeś sprzęt");
                Console.WriteLine("Czy jest to część 5C? 1) Tak | (Dowolny klawisz) Nie");
                var choice2 = Console.ReadLine();
                bool isPartOf5C = choice2 == "1";
                GearItem gearItem = new GearItem(name, weightInGrams, isPartOf5C);
                
                var fiveCCategories = new List<ItemCategory>
                {
                    ItemCategory.CuttingTool,
                    ItemCategory.FireStarter,
                    ItemCategory.Shelter,
                    ItemCategory.WaterContainer,
                    ItemCategory.Cordage
                };
                
                if (isPartOf5C)
                {
                    Console.WriteLine("Wybierz kategorię: ");
                    for (int i = 0; i < fiveCCategories.Count; i++)
                        Console.WriteLine($"{i + 1}. {fiveCCategories[i].ToPolish()}");
                    if (!int.TryParse(Console.ReadLine(), out int categoryNumber)
                        || categoryNumber < 1 || categoryNumber > fiveCCategories.Count)
                    {
                        Console.WriteLine("Błędny numer");
                        return;
                    };
                    gearItem.Category = fiveCCategories[categoryNumber - 1];
                }
                else
                {
                    Console.WriteLine("Wybierz kategorię sprzętu: ");
                    var nonFiveCCategories = Enum.GetValues<ItemCategory>()
                        .Where(c => !fiveCCategories.Contains(c))
                        .ToList();
                    for (int i = 1; i < nonFiveCCategories.Count; i++)
                        Console.WriteLine($"{i+1}. {nonFiveCCategories[i].ToPolish()}");

                    Console.WriteLine("Wprowadź indeks: ");
                    if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 1 ||
                        categoryIndex > nonFiveCCategories.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks kategorii...");
                        return;
                    }
                    gearItem.Category = nonFiveCCategories[categoryIndex - 1];
                }
                
                
              
                
                Console.WriteLine("Ile godzin był uzywany?");
                if (!double.TryParse(Console.ReadLine(), out var hoursUsed))
                {
                    Console.WriteLine("Nieprawidłowa wartość. Naciśnij klawisz...");
                    Console.ReadKey();
                    return;
                }
                gearItem.UsageHours = hoursUsed;
                _inventoryService.AddItem(gearItem);
                Console.WriteLine($"Dodano: {gearItem.GetSummary()}");
                Console.ReadKey();
                break;
        }
    }

    private void ShowItemsNeedingService()
    {
        var items = _inventoryService.GetItemsNeedingService();
        if (items.Count == 0)
        {
            Console.WriteLine("Brak przedmiotów wymagających naprawy");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.GetSummary());
            }
        }
        
    }

    private void RemoveItem()
    {
        var items = _inventoryService.GetAll();

        if (items.Count == 0)
        {
            Console.WriteLine("Lista przedmiotów jest pusta.");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine($"{items.IndexOf(item) + 1}. {item.Name} - {item.Condition.ToPolish()} - {item.WeightGrams}g");
            }
            Console.WriteLine("Wybierz numer przedmiotu, który chcesz usunąć");
            if (!int.TryParse(Console.ReadLine(), out int itemToRemove))
            {
                Console.WriteLine("Nieprawidłowa wartość. Naciśnij klawisz...");
                Console.ReadKey();
                return;
            }

            if (itemToRemove < 1 || itemToRemove > items.Count)
            {
                Console.WriteLine("ID spoza zakresu");
                return;
            }
            var itemToRemoveId = items[itemToRemove - 1].Id;
            try
            {
                _inventoryService.RemoveItem(itemToRemoveId);
                Console.WriteLine("Przedmiot usunięty.");
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();

        }
    }
}