using OffGrid.Models.Enums;

namespace OffGrid.UI;

public static class EnumExtensions
{
    public static string ToPolish(this ItemCategory category) => category switch
    {
        ItemCategory.CuttingTool    => "Narzędzie tnące",
        ItemCategory.FireStarter    => "Krzesiwo/Ogień",
        ItemCategory.Shelter        => "Schronienie",
        ItemCategory.WaterContainer => "Pojemnik na wodę",
        ItemCategory.Cordage        => "Linka/Sznur",
        ItemCategory.Food           => "Jedzenie",
        ItemCategory.Water          => "Woda",
        ItemCategory.Medical        => "Medyczny",
        ItemCategory.Navigation     => "Nawigacja",
        ItemCategory.Clothing       => "Odzież",
        ItemCategory.Tools          => "Narzędzia",
        ItemCategory.Other          => "Inne",
        _                           => category.ToString()
    };

    public static string ToPolish(this ItemCondition condition) => condition switch
    {
        ItemCondition.Excellent    => "Doskonały",
        ItemCondition.Good         => "Dobry",
        ItemCondition.NeedsService => "Wymaga serwisu",
        ItemCondition.Damaged      => "Uszkodzony",
        _                          => condition.ToString()
    };

    public static string ToPolish(this SkillLevel level) => level switch
    {
        SkillLevel.NotLearned    => "Nieznana",
        SkillLevel.Beginner      => "Początkujący",
        SkillLevel.Intermediate  => "Średniozaawansowany",
        SkillLevel.Advanced      => "Zaawansowany",
        _                        => level.ToString()
    };

    public static string ToPolish(this AlertPriority priority) => priority switch
    {
        AlertPriority.Low      => "Niski",
        AlertPriority.Medium   => "Średni",
        AlertPriority.High     => "Wysoki",
        AlertPriority.Critical => "Krytyczny",
        _                      => priority.ToString()
    };
}