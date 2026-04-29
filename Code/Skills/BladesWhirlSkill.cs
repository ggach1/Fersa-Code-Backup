using System.Collections.Generic;
using UnityEngine;
using YIS.Code.Combat;
using YIS.Code.Skills;
using YIS.Code.Skills.Interfaces;
using YIS.Code.Skills.Sequences;

namespace CIW.Code.Skills
{
    public class BladesWhirlSkill : BaseSkill, IAttackSkill
    {
        bool _onChain = false;

        public override float GetFinalDamage()
        {
            float baseDmg = SkillData.damage;

            if (_onChain)
            {
                return baseDmg * 1.5f;
            }

            return baseDmg;
        }

        protected override IReadOnlyList<ISkillAction> NormalSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            List<ISkillAction> actions = new List<ISkillAction>();
            
            DamageData damageData = new DamageData(GetFinalDamage(), CurrentElementalState.CurrentElemental);
            
            actions.Add(new PlayEffectCallbackAction(this, targets));
            actions.Add(new DamageSkillAction(user, targets, damageData, SkillData.impulsePower));
            
            _onChain = true;
            return actions;
        }

        protected override IReadOnlyList<ISkillAction> ChainSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            List<ISkillAction> actions = new List<ISkillAction>();
            
            DamageData damageData = new DamageData(GetFinalDamage(), CurrentElementalState.CurrentElemental);
            
            actions.Add(new PlayEffectCallbackAction(this, targets));
            actions.Add(new DamageSkillAction(user, targets, damageData, SkillData.impulsePower));
            
            _onChain = true;
            return actions;
        }

        public void UseAttackSkill()
        {
        }
    }
}