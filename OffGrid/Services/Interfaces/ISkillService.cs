using OffGrid.Models;
using OffGrid.Models.Enums;

namespace OffGrid.Services.Interfaces;

public interface ISkillService
{
    void AddSkill(Skill skill);
    void RemoveSkill(Guid id);
    Skill GetById(Guid id);
    List<Skill> GetAll();
    List<Skill> GetByCategory(SkillCategory category);
    List<Skill> GetSkillByLevel(SkillLevel level);
    Skill UpdateSkill(Skill skill, SkillLevel level);
    Skill AddSkillNote(Guid skillId, string note);
    List<Skill> GetNeglectedSkills(int daysSinceLastPractice);
    List<Skill> GetNotLearnedSkills();
    
}