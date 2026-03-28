# OffGrid
## Projekt aplikacji 

**Autor:** Karol Bąk
**Przedmiot:** Praktyka zawodowa – Informatyka III rok  
**Prowadząca:** dr inż. Karolina Czarnacka  

## Cel projektu

**Cel projektu**  
Stworzenie konsolowej aplikacji .NET do zarządzania zasobami w stylu off-grid / samowystarczalności (bushcraft, homestead, survival). Aplikacja pomaga śledzić inwentarz (plecak + zapasy domowe), planować zadania, monitorować umiejętności oraz szacować autonomię zasobów (ile dni przeżyjesz bez sklepu).

**Project goal**  
Create a simple .NET console application for managing off-grid / self-sufficiency resources (bushcraft, homestead, survival). It tracks unified inventory, tasks/checklists, skills progress and resource autonomy forecasts.

## Główny użytkownik / Main user
Osoba prywatna rozwijająca styl życia off-grid, bushcraft i samowystarczalność (autor projektu).  
Private individual building off-grid / self-sufficient lifestyle (the project author).

## Problem rozwiązywany / Problem solved
Chaos w zasobach → zapominanie co jest w zapasach/plecaku, brak planu zadań, brak widoczności postępów w umiejętnościach i realnej autonomii (woda, jedzenie, prąd).

## Kluczowe funkcje (MVP – 5 funkcji na semestr)

1. Unified Inventory – jeden magazyn zasobów (przedmioty, żywność, woda, energia – stan, lokalizacja, data ważności)  
2. Checklists & Planner – listy pakowania na wyjazdy + kalendarz zadań domowych/off-grid  
3. Activity & Skill Tracker – dziennik aktywności + śledzenie opanowanych umiejętności (bushcraft/homestead)  
4. Resource Dashboard – aktualny stan + prognoza dni autonomii  
5. Motivation & Reports – proste podsumowania miesięczne + odznaki/streak

## Technologia
- .NET 8 Console Application  
- Entity Framework Core + SQLite (lokalna baza w pliku .db)  
- Pakiety: Microsoft.EntityFrameworkCore.Sqlite, ConsoleTables

## Struktura projektu
```
OffGrid/
├── Models/          ← klasy danych i enumy
├── Services/        ← logika biznesowa + interfejsy
├── UI/              ← interfejs konsolowy
├── Data/            ← warstwa trwałości danych (planowana)
└── Program.cs
```

## Postęp prac
- [x] Etap 1 – Modele danych (Item, FoodItem, WaterItem, GearItem,
  Backpack, UserProfile, HouseholdProfile, Skill, ExpeditionPlan)
- [x] Etap 2 – Logika biznesowa (InventoryService, BackpackService,
  AutonomyCalculator, SkillService, ExpeditionService)
- [x] Etap 3 – Interfejs konsolowy (w trakcie)
- [ ] Etap 4 – Trwałość danych (SQLite)
- [ ] Etap 5 – Generowanie PDF
- [ ] Etap 6 – Dopracowanie i raport końcowy


Prowadząca dodana jako collaborator: @KCzarnackaWSEI  
