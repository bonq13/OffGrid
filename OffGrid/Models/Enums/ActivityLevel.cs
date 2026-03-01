namespace OffGrid.Models.Enums;

public enum ActivityLevel
{
    // Odpowiada współczynnikom PAL z norm WHO
    Resting,        // PAL 1.2 – baza stacjonarna, odpoczynek
    Light,          // PAL 1.5 – lekka aktywność, krótkie spacery
    Moderate,       // PAL 1.75 – marsz z plecakiem
    Heavy           // PAL 2.0+ – ciężka praca fizyczna, budowa schronienia
}