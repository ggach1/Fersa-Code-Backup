using UnityEngine;
using YIS.Code.Skills;

namespace CIW.Code.Player.SkillCompo
{
    public interface IRecipeChecker
    {
        bool TryGetChainingSkill(SkillDataSO[] inputSkills, out SkillDataSO combinationSkill);
    }
}

