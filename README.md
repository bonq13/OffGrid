# OffGrid
## Projekt aplikacji

**Autor:** Karol Bąk
**Przedmiot:** Praktyka zawodowa – Informatyka III rok  
**Prowadząca:** dr inż. Karolina Czarnacka

## Cel projektu

Stworzenie konsolowej aplikacji .NET do zarządzania zasobami w stylu
off-grid / samowystarczalności (bushcraft, homestead, survival).
Aplikacja pomaga śledzić inwentarz (plecak + zapasy domowe), planować
wyprawy bushcraftowe, monitorować umiejętności survivalowe oraz szacować
autonomię zasobów dla całego gospodarstwa domowego.

## Główny użytkownik
Pasjonat bushcraftu, survivalu i preperingu rozwijający styl życia
off-grid i samowystarczalność.

## Problem rozwiązywany
Brak jednego narzędzia łączącego zarządzanie ekwipunkiem survivalowym,
planowanie wypraw, śledzenie umiejętności bushcraftowych oraz monitoring
zapasów dla całego gospodarstwa domowego z kalkulatorem autonomii.

## Funkcjonalności

1. **Inwentarz** – zarządzanie sprzętem, jedzeniem i wodą z filtrowaniem
   po kategorii i stanie technicznym, walidacja reguły 5C Canterbury'ego
2. **Plecak** – tworzenie wirtualnych plecaków, kontrola wagi,
   walidacja systemu 5C przy pakowaniu
3. **Autonomia** – kalkulator przetrwania dla gospodarstwa domowego
   oparty na normach WHO PAL, system alertów priorytetowych
4. **Umiejętności** – śledzenie postępu umiejętności survivalowych
   z wykrywaniem zaniedbanych umiejętności
5. **Wyprawy** – planowanie wypraw bushcraftowych z zarządzaniem trasą
   i generowaniem raportu PDF

## Technologia
- .NET 8 Console Application
- Architektura warstwowa (Models / Services / UI / Data)
- JSON – lokalna persystencja danych (System.Text.Json)
- Interfejsy serwisów przygotowane pod przyszłą rozbudowę web/mobile
- QuestPDF – generowanie raportów wypraw (planowane)

## Struktura projektu

```
OffGrid/
├── Models/
│   └── Enums/
├── Services/
│   └── Interfaces/
├── UI/
├── Data/
│   └── Interfaces/
└── Program.cs
```


## Postęp prac
- [x] Etap 1 – Modele danych (Item, FoodItem, WaterItem, GearItem,
  Backpack, UserProfile, HouseholdProfile, Skill, ExpeditionPlan)
- [x] Etap 2 – Logika biznesowa (InventoryService, BackpackService,
  AutonomyCalculator, SkillService, ExpeditionService)
- [x] Etap 3 – Interfejs konsolowy (wszystkie widoki zaimplementowane)
- [x] Etap 4 – Trwałość danych (JSON z polimorficzną serializacją)
- [ ] Etap 5 – Generowanie PDF
- [ ] Etap 6 – Dopracowanie i raport końcowy

## Prowadząca
Prowadząca dodana jako collaborator: @KCzarnackaWSEI