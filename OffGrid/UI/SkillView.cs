using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.UI;

public class SkillView
{
    private readonly ISkillService _skillService;
    
    public SkillView(ISkillService skillService)
    {
        _skillService = skillService;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Umiejętności ===");
            Console.WriteLine("1. Dodaj umiejętność");
            Console.WriteLine("2. Usuń umiejętność");
            Console.WriteLine("3. Wyświetl wszystkie");
            Console.WriteLine("4. Wyszukaj po kategorii");
            Console.WriteLine("5. Wyszukaj po poziomie");
            Console.WriteLine("6. Wyświetl dawno niepraktykowane");
            Console.WriteLine("7. Wyświetl jeszcze niepoznane");
            Console.WriteLine("8. Zaktualizuj umiejętność");
            Console.WriteLine("9. Dodaj notatkę");
            Console.WriteLine("0. Powrót");
            Console.Write("\nWybierz opcję:");

            switch (Console.ReadLine())
            {
                case "1": AddSkill(); break;
                case "2": RemoveSkill(); break;
                case "3": ShowAllSkills(); break;
                case "4": SearchByCategory(); break;
                case "5": SearchByLevel(); break;
                case "6": ShowRecentlyNotPracticed(); break;
                case "7": ShowNotPracticedYet(); break;
                case "8": UpdateSkill(); break;
                case "9": AddNote(); break;
                case "0": return;
                default:
                    Console.WriteLine("Nieznana opcja, naciśnij dowolny klawisz...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void AddSkill()
    {
        Console.WriteLine("Wybierz kategorię umiejętności, którą chcesz dodać:");
        var categories = Enum.GetValues(typeof(SkillCategory)).Cast<SkillCategory>().ToList();
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i+1}. {categories[i].ToPolish()}");
        }

        if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || categoryChoice < 1 ||
            categoryChoice > categories.Count)
        {
            Console.WriteLine("Niepoprawny format numeru");
            Console.ReadKey();
            return;
        }
        var category = categories[categoryChoice - 1];
        Console.WriteLine("Podaj nazwę umiejętności:");
        var skillName = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Podaj opis umiejętności: ");
        var skillDescription = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("W jakim stopniu opanowałeś/aś umiejętność?");
        var skillLevelCat = Enum.GetValues(typeof(SkillLevel)).Cast<SkillLevel>().ToList();
        for (int i = 0; i < skillLevelCat.Count; i++)
        {
            Console.WriteLine($"{i+1}. {skillLevelCat[i].ToPolish()}");
        }

        if (!int.TryParse(Console.ReadLine(), out int skillLevelChoice) || skillLevelChoice < 1 ||
            skillLevelChoice > skillLevelCat.Count)
        {
            Console.WriteLine("Niepoprawny format numeru");
            Console.ReadKey();
            return;
        }
        var skillLevel = skillLevelCat[skillLevelChoice - 1];
        
        Skill skill = new Skill(skillName, skillDescription, category, skillLevel);
        _skillService.AddSkill(skill);
        Console.WriteLine("Umiejętność dodana!");

        Console.ReadKey();


    }
    
    private void RemoveSkill()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności.");
            return;
        }
        
            Console.WriteLine("Którą umiejętność chcesz usunąć? Podaj numer: ");
            for (int i = 0; i < skills.Count; i++)
            {
                Console.WriteLine($"{i+1}. {skills[i].Name}");
            }
            if (!int.TryParse(Console.ReadLine(), out int skillToRemove))
            {
                Console.WriteLine("Nieprawidłowa wartość. Naciśnij klawisz...");
                Console.ReadKey();
                return;
            }
            if (skillToRemove < 1 || skillToRemove > skills.Count)
            {
                Console.WriteLine("ID spoza zakresu");
                return;
            }
            var skillToRemoveId = skills[skillToRemove - 1].Id;
            
            try
            {
                Console.WriteLine($"Czy na pewno chcesz usunąć {skills[skillToRemove - 1].Name}? [T/N]");
                if (Console.ReadLine() != "T") return;
                _skillService.RemoveSkill(skillToRemoveId);
                Console.WriteLine("Przedmiot usunięty.");
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
    }
    
    private void ShowAllSkills()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętnośći do wyświetlenia!");
            return;
        }
        
        for (int i = 0; i < skills.Count; i++)
            {
                var lastPractice = skills[i].LastPracticeDate?.ToString("dd.MM.yyyy") ?? "Nigdy";
                Console.WriteLine($"{i+1}. {skills[i].Name} | {skills[i].Category.ToPolish()} | " +
                                  $"{skills[i].Level.ToPolish()} | Ostatnio ćwiczone: {lastPractice}");
            }
        Console.ReadKey();
    }
    
    private void SearchByCategory()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności do wyszukania.");
            return;
        }
            var categories = Enum.GetValues(typeof(SkillCategory)).Cast<SkillCategory>().ToList();
            Console.WriteLine("Wybierz kategorie:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i+1}. {categories[i].ToPolish()} ({skills.Count(s => s.Category == categories[i])})");
            }
            
            if (!int.TryParse(Console.ReadLine(), out int categoryNumber) 
                || categoryNumber < 1 || categoryNumber > categories.Count)
            {
                Console.WriteLine("Nieprawidłowy wybór.");
                Console.ReadKey();
                return;
            }
            
            var chosenCategory = categories[categoryNumber - 1];
            
            var skillsByCategory = skills.Where(skill => skill.Category == chosenCategory).ToList();

            foreach (var skill in skillsByCategory)
            {
                Console.WriteLine($"{skill.Name} | {skill.Level.ToPolish()}");
            }
            Console.ReadKey();
    }
    
    private void SearchByLevel()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności do wyszukania.");
            return;
        }
        
            var skillLevels = Enum.GetValues(typeof(SkillLevel)).Cast<SkillLevel>().ToList();
            Console.WriteLine("Jaki poziom umiejętności cię interesuje?: ");
            for (int i = 0; i < skillLevels.Count; i++)
            {
                Console.WriteLine($"{i+1}. {skillLevels[i].ToPolish()} ({skills.Count(s => s.Level == skillLevels[i])})");
            }

            if (!int.TryParse(Console.ReadLine(), out var skillLevelNumber) || skillLevelNumber < 1 ||
                skillLevelNumber > skillLevels.Count)
            {
                Console.WriteLine("Nieprawidłowy wybór.");
                Console.ReadKey();
                return;
            }
            
            var chosenSkillLevel = skillLevels[skillLevelNumber - 1];
            var skillsByLevel = skills.Where(skill => skill.Level == chosenSkillLevel).ToList();
            
            foreach (var skill in skillsByLevel)
            {
                Console.WriteLine($"{skill.Name} | {skill.Level.ToPolish()}");
            }
            Console.ReadKey();
        
    }
    
    private void ShowRecentlyNotPracticed()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności do wyświetlenia.");
            return;
        }
            Console.WriteLine("Podaj liczbę dni: ");
            if (!int.TryParse(Console.ReadLine(), out int daysSincePractice) || daysSincePractice < 1) 
            {
                Console.WriteLine("Nieprawidłowy wybór.");
                Console.ReadKey();
                return;
            }
            
            var itemsByDaysSincePractice = skills
                .Where(skill => skill.DaysSinceLastPractice.HasValue && 
                                skill.DaysSinceLastPractice.Value >= daysSincePractice)
                .ToList();
            foreach (var skill in itemsByDaysSincePractice)
            {
                Console.WriteLine($"{skill.Name} | {skill.Category.ToPolish()} | {skill.Level.ToPolish()}");
            }
            Console.ReadKey();
        
    }
    
    private void ShowNotPracticedYet()
    {
        var skills = _skillService.GetAll().Where(skill => skill.LastPracticeDate == null).ToList();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności do wyświetlenia.");
            return;
        }
        else
        {
            foreach (var skill in skills)
            {
                Console.WriteLine($"{skill.Name} | {skill.Category.ToPolish()} | {skill.Level.ToPolish()}");
            }
        }
        Console.ReadKey();
    }
    
    private void UpdateSkill()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności do wyświetelnia.");
            return;
        }
        Console.WriteLine("Wybierz umiejętność do edytowania: ");
        for (int i = 0; i < skills.Count; i++)
        {
            Console.WriteLine($"{i+1}. {skills[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out int skillNumber) || skillNumber < 1 || skillNumber > skills.Count)
        {
            Console.WriteLine("Nieprawidłowy wybór.");
            Console.ReadKey();
            return;
        }

        var choosenSkill = skills[skillNumber - 1];
        Console.WriteLine("Co chcesz edytować?");
        Console.WriteLine("1. Nazwę");
        Console.WriteLine("2. Opis");
        Console.WriteLine("3. Data ostatniej praktyki");
        Console.WriteLine("4. Notatki");
        Console.WriteLine("5. Poziom");
        Console.WriteLine("6. Kategorię");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Nowa nazwa: ");
                var newSkillName = Console.ReadLine() ?? string.Empty;
                choosenSkill.Name = newSkillName;
                break;
            case "2":
                Console.WriteLine("Nowy opis: ");
                var newSkillDescription = Console.ReadLine() ?? string.Empty;
                choosenSkill.Description = newSkillDescription;
                break;
            case "3":
                Console.WriteLine("Nowa data ostatniej praktyki: ");
                var newLastPracticeDate = Console.ReadLine();
                if (!DateTime.TryParse(newLastPracticeDate, out DateTime newLastPracticeDateParsed))
                {
                    Console.WriteLine("Nieprawidłowy format daty.");
                    Console.ReadKey();
                    return;
                }
                choosenSkill.LastPracticeDate = newLastPracticeDateParsed;
                break;
            case "4":
                Console.WriteLine("Notatki: ");
                var newNotes = Console.ReadLine();
                choosenSkill.Notes = newNotes;
                break;
            case "5":
                Console.WriteLine("Wybierz nowy poziom: ");
                var skillLevels = Enum.GetValues(typeof(SkillLevel)).Cast<SkillLevel>().ToList();
                for (int i = 0; i < skillLevels.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {skillLevels[i].ToPolish()} ({skills.Count(s => s.Level == skillLevels[i])})");
                }

                if (!int.TryParse(Console.ReadLine(), out var skillLevelNumber) || skillLevelNumber < 1 ||
                    skillLevelNumber > skillLevels.Count)
                {
                    Console.WriteLine("Nieprawidłowy wybór.");
                    Console.ReadKey();
                    return;
                }
            
                var chosenSkillLevel = skillLevels[skillLevelNumber - 1];
                choosenSkill.Level = chosenSkillLevel;
                break;
            case "6":
                Console.WriteLine($"Dotychczasowa kategoria {choosenSkill.Name} to {choosenSkill.Category.ToPolish()}");
                Console.WriteLine("Wybierz nową kategorię");
                var categories = Enum.GetValues(typeof(SkillCategory)).Cast<SkillCategory>().ToList();
                Console.WriteLine("Wybierz kategorie:");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {categories[i].ToPolish()} ({skills.Count(s => s.Category == categories[i])})");
                }
            
                if (!int.TryParse(Console.ReadLine(), out int categoryNumber) 
                    || categoryNumber < 1 || categoryNumber > categories.Count)
                {
                    Console.WriteLine("Nieprawidłowy wybór.");
                    Console.ReadKey();
                    return;
                }
            
                var chosenCategory = categories[categoryNumber - 1];
                choosenSkill.Category = chosenCategory;
                break;
            default:
                Console.WriteLine("Wybierz z dostępnych opcji...");
                break;
        }
        Console.ReadKey();
    }
    
    private void AddNote()
    {
        var skills = _skillService.GetAll();
        if (skills.Count == 0)
        {
            Console.WriteLine("Brak umiejętności do wyświetlenia.");
            return;
        }
        
        Console.WriteLine("Wybierz umiejętność, do której chcesz dodać notatkę: ");
        for (int i = 0; i < skills.Count; i++)
        {
            Console.WriteLine($"{i+1}. {skills[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out int chosenSkillNumber) || chosenSkillNumber < 1 ||
            chosenSkillNumber > skills.Count)
        {
            Console.WriteLine("Nieprawidłowy wybór.");
            Console.ReadKey();
            return;
        }
        
        var chosenSkill = skills[chosenSkillNumber - 1];
        Console.WriteLine("Podaj treść notatki: ");
        chosenSkill.Notes = Console.ReadLine();
        Console.WriteLine("Notatka dodana!");
        Console.ReadKey();
    }
    
}