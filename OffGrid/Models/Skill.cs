using OffGrid.Models.Enums;

namespace OffGrid.Models;

public class Skill
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? LastPracticeDate { get; set; }
    public string? Notes { get; set; }
    public SkillCategory Category { get; set; } = SkillCategory.Other;
    public SkillLevel Level { get; set; } = SkillLevel.NotLearned;
    
    public int? DaysSinceLastPractice => LastPracticeDate.HasValue
        ? (DateTime.Now - LastPracticeDate.Value).Days
        : null;

    public Skill(string name, string description, SkillCategory category, SkillLevel level)
    {
        Name = name;
        Description = description;
        Category = category;
        Level = level;
    }
}