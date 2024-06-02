using System;
using ProjectMIL.GameEvent;

namespace ProjectMIL.Combat
{
    public class DamageHandler
    {
        public DamageHandler()
        {
            EventBus.Subscribe<OnAttackCasted>(OnAttackEventRaised);
        }

        private void OnAttackEventRaised(OnAttackCasted cast)
        {
            CombatActor attacker = CombatActorContainer.GetActorByInstanceID(cast.attackerActorInstanceID);
            CombatActor target = CombatActorContainer.GetActorByInstanceID(cast.targetActorInstanceID);

            if (attacker == null || target == null)
                return;

            if (UnityEngine.Random.Range(0f, 100f) <= 50f)
            {
                UnityEngine.Debug.Log("MISS");
                return;
            }

            float rawDamage = (float)attacker.Info.Attack * 1f; // TODO: handle multipliers
            float damage = rawDamage * ((float)attacker.Info.Attack / (float)(attacker.Info.Attack + target.Info.Defense));

            float criticalChance = 100f * ((float)attacker.Info.Critical / (float)(attacker.Info.Critical + target.Info.CriticalResistance));
            float finalDamage = damage;

            if (UnityEngine.Random.Range(0f, 100f) < criticalChance)
            {
                finalDamage *= 1.5f; // TODO: handle critical damage multiplier
            }

            EventBus.Publish(new OnDamageCalculated
            {
                attackerActorInstanceID = cast.attackerActorInstanceID,
                targetActorInstanceID = cast.targetActorInstanceID,
                damage = Convert.ToInt32(finalDamage),
                hitPosition = cast.hitPosition
            });
        }
    }
}