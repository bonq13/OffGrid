using OffGrid.Models;
using OffGrid.Services;
using OffGrid.Services.Interfaces;

namespace OffGrid.UI;

public class ExpeditionView
{
    private readonly IBackpackService _backpackService;
    private readonly IExpeditionService _expeditionService;
    
    private readonly List<ExpeditionPlan> _expeditions = new List<ExpeditionPlan>();
    private readonly List<Backpack> _backpacks;

    public ExpeditionView(IBackpackService backpackService, 
        IExpeditionService expeditionService,
        List<Backpack> backpacks)
    {
        _backpackService = backpackService;
        _expeditionService = expeditionService;
        _backpacks = backpacks;
    }
    
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Wyprawy ===");
            Console.WriteLine("1. Dodaj nową wprawę");
            Console.WriteLine("2. Usuń wyprawę");
            Console.WriteLine("3. Wyszukaj wyprawę");
            Console.WriteLine("4. Wyświetl wszystkie wyprawy");
            Console.WriteLine("5. Modyfikuj wyprawę");
            Console.WriteLine("6. Generuj raport PDF");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję: ");


            switch (Console.ReadLine())
            {
                case "1": AddExpedition(); break;
                case "2": RemoveExpedition(); break;
                case "3": SearchExpedition(); break;
                case "4": ShowAllExpeditions(); break;
                case "5": ModifyExpedition(); break;
                case "6": GenerateReport(); break;
                case "0": return;
                default: Console.WriteLine("Nieznana opcja."); break;
            }
        }
    }

    private void GenerateReport()
    {
        if (_expeditions.Count == 0)
        {
            Console.WriteLine("Brak wypraw.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Wybierz wyprawę do wygenerowania raportu:");
        for (int i = 0; i < _expeditions.Count; i++)
            Console.WriteLine($"{i + 1}. {_expeditions[i].Name}");

        if (!int.TryParse(Console.ReadLine(), out int choice) 
            || choice < 1 || choice > _expeditions.Count)
        {
            Console.WriteLine("Nieprawidłowy wybór.");
            Console.ReadKey();
            return;
        }

        var expedition = _expeditions[choice - 1];
        var pdfService = new PdfExportService();
        var fileName = $"raport_{expedition.Name}_{DateTime.Now:yyyyMMdd}.pdf";

        try
        {
            pdfService.GenerateExpeditionReport(expedition, fileName);
            Console.WriteLine($"Raport wygenerowany: {Path.GetFullPath(fileName)}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Błąd generowania raportu: {e.Message}");
        }
        Console.ReadKey();
    }
    
    private void AddExpedition()
    {
        Console.WriteLine("Podaj nazwę wyprawy: ");
        var expeditionName = Console.ReadLine();
        
        Console.WriteLine("Podaj opis wyprawy: ");
        var expeditionDescription = Console.ReadLine();
        
        Console.WriteLine("Podaj datę rozpoczęcia wyprawy: ");
        if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
        {
            Console.WriteLine("Niepoprawny format daty...");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Podaj datę końca wyprawy: ");
        if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
        {
            Console.WriteLine("Niepoprawny format daty...");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Podaj przyblizoną długość trasy: ");
        if (!double.TryParse(Console.ReadLine(), out var estimatedDistance))
        {
            Console.WriteLine("Niepoprawny format...");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Podaj liczbę uczestników: ");
        if (!int.TryParse(Console.ReadLine(), out var numberOfParticipants))
        {
            Console.WriteLine("Niepoprawny format...");
            Console.ReadKey();  
            return;
        }
        
        Console.WriteLine("Podaj szacunkowy koszt: ");
        if (!double.TryParse(Console.ReadLine(), out var estimatedCost))
        {
            Console.WriteLine("Niepoprawny format...");
            Console.ReadKey();
            return;
        }

        if (_backpacks.Count == 0)
        {
            Console.WriteLine("Brak plecaków. Najpierw dodaj plecak w module Plecak.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Wybierz plecak wyprawy: ");
        for (int i = 0; i < _backpacks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_backpacks[i].Name} | " +
                              $"maksymalna waga: {_backpacks[i].MaxWeightKg}kg");
        }

        if (!int.TryParse(Console.ReadLine(), out var result) || result < 1 || result > _backpacks.Count)
        {
            Console.WriteLine("Niepoprawna wartość...");
            Console.ReadKey();
            return;
        }
        
        var chosenBackpack = _backpacks[result - 1];

        var (plan, missing5C) = _expeditionService.CreateExpedition(expeditionName, startDate, endDate, chosenBackpack);
        
        plan.EstimatedKilometers = (int)estimatedDistance;
        plan.ExpeditionParticipants = numberOfParticipants;
        plan.EstimatedCost = estimatedCost;
        plan.Notes = expeditionDescription;
        
        _expeditions.Add(plan);


        if (missing5C.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ Uwaga! W plecaku brakuje elementów 5C:");
            foreach (var category in missing5C)
                Console.WriteLine($"  - {category.ToPolish()}");
            Console.ResetColor();
        }

        Console.WriteLine($"Wyprawa '{plan.Name}' dodana pomyślnie.");
        Console.ReadKey();
        
    }

    private void RemoveExpedition()
    {
        if (_expeditions.Count == 0)
        {
            Console.WriteLine("Brak wypraw, nie ma co usuwać.");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Wybierz numer wyprawy do usunięcia:");
        for (int i = 0; i < _expeditions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_expeditions[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out var result) || result < 1 || result > _expeditions.Count)
        {
            Console.WriteLine("Niepoprawna wartość...");
            Console.ReadKey();
            return;
        }

        var expeditionToRemove = _expeditions[result - 1];
        _expeditions.Remove(expeditionToRemove);
        Console.WriteLine($"Usunięto {expeditionToRemove.Name} pomyślnie");
        Console.ReadKey();
    }
    
    private void SearchExpedition()
    {
        if (_expeditions.Count == 0)
        {
            Console.WriteLine("Brak wypraw do wyszukania");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Po czym chcesz wyszukiwać?");
        Console.WriteLine("1. Po nazwie");
        Console.WriteLine("2. Po dacie rozpoczęcia");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Podaj nazwę wyprawy (lub jej fragment)");
                var searchTerm = Console.ReadLine();
                var foundExpedition = _expeditions.Where(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                if (foundExpedition.Count() == 0)
                {
                    Console.WriteLine("Nie znaleziono wyprawy o podanej nazwie");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Znaleziono:");
                foreach (var result in foundExpedition)
                {
                    Console.WriteLine($"{result.Name} | {result.StartDate:dd/MM/yyyy} - {result.EndDate:dd/MM/yyyy} | {result.EstimatedKilometers}km");
                }
                Console.ReadKey();
                break;
            case "2":
                Console.WriteLine("Podaj datę rozpoczęcia wyprawy");
                if(!DateTime.TryParse(Console.ReadLine(), out var startDate))
                {
                    Console.WriteLine("Niepoprawny format daty...");
                    Console.ReadKey();
                    return;
                }
                var foundExpedition2 = _expeditions.Where(e => e.StartDate == startDate);
                if (foundExpedition2.Count() == 0)
                {
                    Console.WriteLine("Nie znaleziono wyprawy o podanej dacie");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Znaleziono:");
                foreach (var result in foundExpedition2)
                {
                    Console.WriteLine($"{result.Name} | {result.StartDate:dd/MM/yyyy} - {result.EndDate:dd/MM/yyyy} | {result.EstimatedKilometers}km");
                }
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Niepoprawna opcja");
                Console.ReadKey();
                break;
        }
    }
    
    private void ShowAllExpeditions()
    {
        if (_expeditions.Count == 0)
        {
            Console.WriteLine("Brak wypraw do wyświetlenia");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < _expeditions.Count; i++)
        {
            Console.WriteLine($"{i+1}. {_expeditions[i].Name} | {_expeditions[i].StartDate:dd/MM/yyyy} - {_expeditions[i].EndDate:dd/MM/yyyy} |  {_expeditions[i].EstimatedKilometers}km");
        }
        
        Console.ReadKey();
    }

    private void ModifyExpedition()
    {
        if (_expeditions.Count == 0)
        {
            Console.WriteLine("Brak wypraw do wyświetlenia");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("Wybierz wyprawę do modyfikacji:");
        for (int i = 0; i < _expeditions.Count; i++)
        {
            Console.WriteLine($"{i+1}. {_expeditions[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out var result) || result < 1 || result > _expeditions.Count)
        {
            Console.WriteLine("Niepoprawna wartość...");
            Console.ReadKey();
            return;
        }
        
        var expeditionToModify = _expeditions[result - 1];
        
        Console.WriteLine("Teraz wybierz co chcesz zmodyfikować: ");
        Console.WriteLine("1. Nazwę");
        Console.WriteLine("2. Datę początkową");
        Console.WriteLine("3. Datę końcową");
        Console.WriteLine("4. Długość trasy (w kilometrach)");
        Console.WriteLine("5. Liczbę uczestników");
        Console.WriteLine("6. Koszt");
        Console.WriteLine("7. Notatki");
        Console.WriteLine("8. Plecak");
        Console.WriteLine("9. Punkty kontrolne");
        Console.WriteLine("0. Wyjście");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Podaj nową nazwę: ");
                expeditionToModify.Name = Console.ReadLine(); 
                break;
            case "2":
                Console.WriteLine("Podaj nową datę początkową: "); 
                if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
                {
                    Console.WriteLine("Niepoprawny format daty...");
                    Console.ReadKey();
                    return;
                }
                expeditionToModify.StartDate = startDate;
                break;
            case "3":
                Console.WriteLine("Podaj nową datę końcową: "); 
                if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
                {
                    Console.WriteLine("Niepoprawny format daty...");
                    Console.ReadKey();
                    return;
                }
                expeditionToModify.EndDate = endDate;
                break;
            case "4":
                Console.WriteLine("Podaj nową długość trasy (km): ");
                if (!int.TryParse(Console.ReadLine(), out var estimatedDistance) || estimatedDistance < 1)
                {
                    Console.WriteLine("Niepoprawna wartość...");
                    Console.ReadKey();
                    return;
                }
                expeditionToModify.EstimatedKilometers = estimatedDistance;
                break;
            case "5":
                Console.WriteLine("Podaj nową liczbę uczestników: "); 
                if (!int.TryParse(Console.ReadLine(), out var numberOfParticipants) || numberOfParticipants < 1)
                {
                    Console.WriteLine("Niepoprawna wartość...");
                    Console.ReadKey();
                    return;
                }
                expeditionToModify.ExpeditionParticipants = numberOfParticipants;
                break;
            case "6":
                Console.WriteLine("Podaj nowy koszt: "); 
                if (!double.TryParse(Console.ReadLine(), out var estimatedCost) || estimatedCost < 0)
                {
                    Console.WriteLine("Niepoprawna wartość...");
                    Console.ReadKey();
                    return;
                }
                expeditionToModify.EstimatedCost = estimatedCost;
                break;
            case "7": 
                Console.WriteLine("Podaj nowe notatki: ");
                expeditionToModify.Notes = Console.ReadLine();
                break;
            case "8":
                Console.WriteLine("Wybierz plecak:");
                for (int i = 0; i < _backpacks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_backpacks[i].Name} | " +
                                      $"maksymalna waga: {_backpacks[i].MaxWeightKg}kg");
                }
                if (!int.TryParse(Console.ReadLine(), out var backpackChoice) 
                    || backpackChoice < 1 || backpackChoice > _backpacks.Count)
                {
                    Console.WriteLine("Niepoprawna wartość...");
                    Console.ReadKey();
                    return;
                }
                expeditionToModify.Gear = _backpacks[backpackChoice - 1];
                break;
            case "9":
                Console.WriteLine("Co chcesz zrobić?");
                Console.WriteLine("1. Dodać punkt na trasie");
                Console.WriteLine("2. Usunąć punkt z trasy");
                Console.WriteLine("3. Edytować punkt z trasy");
                Console.WriteLine("4. Powrót");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Podaj nazwę punktu:");
                        var routePointName = Console.ReadLine();
                        Console.WriteLine("Podaj szerokość geograficzną:");
                        if (!double.TryParse(Console.ReadLine(), out var routePointLatitude))
                        {
                            Console.WriteLine("Niepoprawny format...");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine("Podaj długość geograficzną:");
                        if (!double.TryParse(Console.ReadLine(), out var routePointLongitude))
                        {
                            Console.WriteLine("Niepoprawny format...");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine("Podaj opis punktu:");
                        var routePointDescription = Console.ReadLine();

                        RoutePoint newRoutePoint = new RoutePoint(routePointName, routePointLatitude, routePointLongitude);
                        newRoutePoint.Notes = routePointDescription;
                        expeditionToModify.RoutePoints.Add(newRoutePoint);
                        break;
                        
                    case "2":
                        if (expeditionToModify.RoutePoints.Count == 0)
                        {
                            Console.WriteLine("Brak określonych punktów na trasie");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine("Wybierz punkt do usunięcia: ");
                        for (int i = 0; i < expeditionToModify.RoutePoints.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {expeditionToModify.RoutePoints[i].Name}");
                        }

                        if (!int.TryParse(Console.ReadLine(), out var pointNumber) || pointNumber < 1 ||
                            pointNumber > expeditionToModify.RoutePoints.Count)
                        {
                            Console.WriteLine("Błędny format");
                            Console.ReadKey();
                            return;
                        }
                        
                        RoutePoint routePointToRemove = expeditionToModify.RoutePoints[pointNumber - 1];
                        expeditionToModify.RoutePoints.Remove(routePointToRemove);
                        break;
                    case "3":
                        if (expeditionToModify.RoutePoints.Count == 0)
                        {
                            Console.WriteLine("Brak określonych punktów na trasie");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine("Wybierz punkt do edycji: ");
                        for (int i = 0; i < expeditionToModify.RoutePoints.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {expeditionToModify.RoutePoints[i].Name}");
                        }

                        if (!int.TryParse(Console.ReadLine(), out var pointResult) || pointResult < 1 ||
                            pointResult > expeditionToModify.RoutePoints.Count)
                        {
                            Console.WriteLine("Błędny format");
                            Console.ReadKey();
                            return;
                        }
                        
                        Console.WriteLine("Co chcesz zmienić? ");
                        Console.WriteLine("1. Nazwę");
                        Console.WriteLine("2. Szerokośc");
                        Console.WriteLine("3. Długość");
                        Console.WriteLine("4. Opis");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.WriteLine("Podaj nową nazwę: ");
                                expeditionToModify.RoutePoints[pointResult - 1].Name = Console.ReadLine();
                                break;
                            case "2":
                                Console.WriteLine("Podaj nową szerokość geograficzną: ");
                                if (!double.TryParse(Console.ReadLine(), out var latitude))
                                {
                                    Console.WriteLine("Niepoprawny format...");
                                    Console.ReadKey();
                                    return;
                                }
                                expeditionToModify.RoutePoints[pointResult - 1].Latitude = latitude;
                                break;
                            case "3":
                                Console.WriteLine("Podaj nową długość geograficzną: ");
                                if (!double.TryParse(Console.ReadLine(), out var longitude))
                                {
                                    Console.WriteLine("Niepoprawny format...");
                                    Console.ReadKey();
                                    return;
                                }
                                expeditionToModify.RoutePoints[pointResult - 1].Longitude = longitude;
                                break;
                            case "4":
                                Console.WriteLine("Podaj nowy opis: ");
                                expeditionToModify.RoutePoints[pointResult - 1].Notes = Console.ReadLine();
                                break;
                            default:
                                Console.WriteLine("Nieznana opcja");
                                break;
                        }
                        break;
                    case "4":
                            return;
                    default:
                        Console.WriteLine("Nieznana opcja");
                        break;
                }
                
                break;
        }
        
        
    }
}