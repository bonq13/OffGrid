using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.UI;

public class AutonomyView
{
    private readonly IInventoryService _inventoryService;
    private readonly IAutonomyCalculator _autonomyCalculator;
    
    private HouseholdProfile? _householdProfile;
    
    public AutonomyView(IInventoryService inventoryService, IAutonomyCalculator autonomyCalculator)
    {
        _inventoryService = inventoryService;
        _autonomyCalculator = autonomyCalculator;
    }
    
    public void Run()
    {
        while (true){
            Console.Clear();
            Console.WriteLine("=== Autonomia ===");
            Console.WriteLine("1. Skonfiguruj profil gospodarstwa");
            Console.WriteLine("2. Zarządzaj członkami gospodarstwa");
            Console.WriteLine("3. Raport autonomii");
            Console.WriteLine("4. Sprawdź alerty");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję: ");
            
            var input = Console.ReadLine();

            switch (input)
            {
                case "1": HouseholdSetup(); break;
                case "2": ManageHouseholdMembers(); break;
                case "3": ReportAutonomy(); break;
                case "4": CheckAlerts(); break;
                case "0": return;
                default:
                    Console.WriteLine("Nieznana opcja, naciśnij dowolny klawisz...");
                    Console.ReadKey();
                    break;
            }
            
            
        }
    }

    private void HouseholdSetup()
    {
        Console.WriteLine("Podaj nazwę gospodarstwa: ");
        var name = Console.ReadLine() ?? string.Empty;
    
        Console.WriteLine("Podaj lokalizację bazy: ");
        var location = Console.ReadLine() ?? string.Empty;
    
        _householdProfile = new HouseholdProfile
        {
            Name = name,
            BaseLocation = location
        };
    
        Console.WriteLine($"Profil '{_householdProfile.Name}' skonfigurowany.");
        Console.ReadKey();
    }
    
    private void ManageHouseholdMembers()
    {
        var household = _householdProfile;
        if (household == null)
        {
            Console.WriteLine("Nie skonfigurowano profilu gospodarstwa.");
            Console.ReadKey();
            return;
        }
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {household.Name} ===");
            Console.WriteLine("1. Dodaj członka");
            Console.WriteLine("2. Usuń członka");
            Console.WriteLine("3. Wyświetl członków");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1": AddHouseholdMember(); break;
                case "2": RemoveHouseholdMember(); break;
                case "3": ShowHouseholdMembers(); break;
                case "0": return;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    Console.ReadKey();
                    break;
            }
        }
        
    }
    
    private void AddHouseholdMember()
    {
        if (_householdProfile == null) return;
        
        Console.WriteLine("Podaj imie członka: ");
        var memberName = Console.ReadLine() ?? string.Empty;
        
        Console.WriteLine("Podaj wiek członka w latach: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Niepoprawny format wieku.");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Podaj wagę członka w kilogramach: ");
        if (!double.TryParse(Console.ReadLine(), out double weight))
        {
            Console.WriteLine("Niepoprawny format wagi.");
            Console.ReadKey();
            return;
        }
        
        HouseholdMember member = new HouseholdMember(memberName, age, weight);
        
        Console.WriteLine($"Jaki poziom aktywności ma {member.Name}?");
        Console.WriteLine("1. Niski ");
        Console.WriteLine("2. Mały ");
        Console.WriteLine("3. Przeciętny ");
        Console.WriteLine("4. Wysoki ");

        if (!int.TryParse(Console.ReadLine(), out int activityLevel))
        {
            Console.WriteLine("Podaj liczbę 1-4");
            Console.ReadKey();
            return;
        }

        switch (activityLevel)
        {
            case 1: member.ActivityLevel = ActivityLevel.Resting; break;
            case 2: member.ActivityLevel = ActivityLevel.Light; break;
            case 3: member.ActivityLevel = ActivityLevel.Moderate; break;
            case 4: member.ActivityLevel = ActivityLevel.Heavy; break;
            default:
                Console.WriteLine("Odpowiedź spoza zakresu");
                Console.ReadKey();
                return;
        }
        
        _householdProfile.Members.Add(member);
        Console.WriteLine($"Dodano {member.Name} do gospodarstwa.");
        Console.ReadKey();
        
        
    }
    
    private void RemoveHouseholdMember()
    {
        if (_householdProfile == null) return;
        if (_householdProfile.Members.Count == 0)
        {
            Console.WriteLine("Brak członków");
            Console.ReadKey();
            return;
        }
        
        var members = _householdProfile.Members;
        
        Console.WriteLine("Wybierz członka, którego chcesz usunąć");
        for (int i = 0; i < members.Count; i++)
        {
            Console.WriteLine($"{i+1}. {members[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > members.Count)
        {
            Console.WriteLine("Niepoprawny format numeru");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine($"Usunięto {members[choice - 1].Name} z gospodarstwa.");
        _householdProfile.Members.RemoveAt(choice - 1);
        Console.ReadKey();
    }
    
    private void ShowHouseholdMembers()
    {
        
        if (_householdProfile == null) return;
        if (_householdProfile.Members.Count == 0)
        {
            Console.WriteLine("Brak członków");
            Console.ReadKey();
            return;
        }
        var members = _householdProfile.Members;

        for (int i = 0; i < members.Count; i++)
        {
            Console.WriteLine($"{i+1}. {members[i].Name} | wiek: {members[i].AgeYears} | waga: {members[i].WeightKg}");
        }
        Console.ReadKey();
    }
    
    private void ReportAutonomy()
    {
        if (_householdProfile == null)
        {
            Console.WriteLine("Najpierw skonfiguruj profil gospodarstwa.");
            Console.ReadKey();
            return;
        }

        var foods = _inventoryService.GetAll().OfType<FoodItem>().ToList();
        var water = _inventoryService.GetAll().OfType<WaterItem>().ToList();

        var foodAutonomy = _autonomyCalculator.CalculateFoodAutonomyDays(foods, _householdProfile);
        var waterAutonomy = _autonomyCalculator.CalculateWaterAutonomyDays(water, _householdProfile);
        
        Console.WriteLine($"Autonomia jedzenia na: {foodAutonomy} dni");
        Console.WriteLine($"Autonomia wody na: {waterAutonomy} dni");
        Console.ReadKey();
    }
    
    private void CheckAlerts()
    {
        if (_householdProfile == null)
        {
            Console.WriteLine("Najpierw skonfiguruj profil gospodarstwa.");
            Console.ReadKey();
            return;
        }

        var foods = _inventoryService.GetAll().OfType<FoodItem>().ToList();
        var water = _inventoryService.GetAll().OfType<WaterItem>().ToList();
        
        var alerts = _autonomyCalculator.GenerateAlerts(foods, water, _householdProfile);
        
        foreach (var alert in alerts)
        {
            Console.ForegroundColor = alert.Priority switch
            {
                AlertPriority.Critical => ConsoleColor.Red,
                AlertPriority.High     => ConsoleColor.Yellow,
                AlertPriority.Medium   => ConsoleColor.Cyan,
                _                      => ConsoleColor.White
            };
            Console.WriteLine($"[{alert.Priority.ToPolish()}] {alert.Message}");
            Console.ResetColor();
        }
        
        if (alerts.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Brak alertów – zapasy na dobrym poziomie.");
            Console.ResetColor();
        }
        
        Console.ReadKey();
    }
    
    
}