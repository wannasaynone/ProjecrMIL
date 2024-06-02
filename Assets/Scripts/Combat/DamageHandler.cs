using System;
using ProjectMIL.GameEvent;

namespace ProjectMIL.Combat
{
    public class DamageHandler
    {
        public DamageHandler()
        {
            EventBus.Subscribe<OnHit>(OnHitEventRaised);
        }

        private void OnHitEventRaised(OnHit hit)
        {
            CombatActor attacker = CombatActorContainer.GetActorByInstanceID(hit.attackerActorInstanceID);
            CombatActor target = CombatActorContainer.GetActorByInstanceID(hit.targetActorInstanceID);

            if (attacker == null || target == null)
                return;

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
                attackerActorInstanceID = hit.attackerActorInstanceID,
                targetActorInstanceID = hit.targetActorInstanceID,
                damage = Convert.ToInt32(finalDamage)
            });
        }
    }
}