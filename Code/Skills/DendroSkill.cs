using System.Collections.Generic;
using PSB.Code.BattleCode.Skills;
using UnityEngine;
using YIS.Code.Combat;
using YIS.Code.Defines;
using YIS.Code.Skills;
using YIS.Code.Skills.Interfaces;
using YIS.Code.Skills.Sequences;

namespace CIW.Code.Skills
{
    public class DendroSkill : BaseSkill, IEnchantProvider
    {
        protected override IReadOnlyList<ISkillAction> NormalSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            List<ISkillAction> actions = new List<ISkillAction>();
            
            DamageData damageData = new DamageData(SkillData.damage, CurrentElementalState.CurrentElemental);
            
            actions.Add(new PlayEffectCallbackAction(this, targets));
            actions.Add(new DamageSkillAction(user, targets, damageData, SkillData.impulsePower));
            
            SkipDamageThisCast = true;

            return actions;
        }

        protected override IReadOnlyList<ISkillAction> ChainSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            List<ISkillAction> actions = new List<ISkillAction>();
            
            actions.Add(new PlayEffectCallbackAction(this, targets));
            actions.Add(new ImmediatelyEnchantSkillAction(user, this, CurrentElementalState.CurrentElemental));
            SkipDamageThisCast = true;
            Debug.Log("풀 체이닝 발동! 속성을 부여하겠습니다.");
            
            return actions;
        }
        
        public override bool SkipAnim(bool isChain)
        {
            return isChain;
        }
        
    }
}
