using OffGrid.Models;
using OffGrid.Models.Enums;
using OffGrid.Services.Interfaces;

namespace OffGrid.Services;

public class SkillService : ISkillService
{
    private readonly List<Skill> _skills = new List<Skill>();    
    
    public void AddSkill(Skill skill)
    {
        if (skill == null) throw new ArgumentNullException(nameof(skill));
        _skills.Add(skill);
    }

    public void RemoveSkill(Guid id)
    {
        var skill = _skills.FirstOrDefault(item => item.Id == id);
        if (skill == null)
        {
            throw new KeyNotFoundException($"Umiejętność o id: {id} nie istnieje!");
        }
        _skills.Remove(skill);
    }

    public Skill GetById(Guid id)
    {
        var skill = _skills.FirstOrDefault(skill => skill.Id == id);
        if (skill == null)
        {
            throw new KeyNotFoundException($"Umiejętność o id: {id} nie istnieje!");
        }
        return skill;
    }
    
    public List<Skill> GetAll()
    {
        return _skills.ToList();
    }

    public List<Skill> GetByCategory(SkillCategory category)
    {
        return _skills.Where(skill => skill.Category == category).ToList();
    }

    public List<Skill> GetSkillByLevel(SkillLevel level)
    {
        return _skills.Where(skill => skill.Level == level).ToList();
    }

    public Skill UpdateSkill(Skill skill, SkillLevel level)
    {
        var existing = _skills.FirstOrDefault(s => s.Id == skill.Id);
        if (existing == null)
            throw new KeyNotFoundException($"Umiejętność o id: {skill.Id} nie istnieje!");
        existing.Level = level;
        return existing;
    }

    public Skill AddSkillNote(Guid skillId, string note)
    {
        var skill = _skills.FirstOrDefault(s => s.Id == skillId);
        if (skill == null)
            throw new KeyNotFoundException($"Umiejętność o id: {skillId} nie istnieje!");
        skill.Notes = note;
        return skill;
    }

    public List<Skill> GetNeglectedSkills(int daysSinceLastPractice)
    {
        return _skills
            .Where(s => s.DaysSinceLastPractice.HasValue && 
                        s.DaysSinceLastPractice.Value >= daysSinceLastPractice)
            .ToList();
    }
    public List<Skill> GetNotLearnedSkills()
    {
        return _skills.FindAll(s => s.Level == SkillLevel.NotLearned);
    }
}