using System;
using ProjectMIL.GameEvent;

namespace ProjectMIL.Combat
{
    public class DamageHandler : IDisposable
    {
        private bool disposed = false;

        public DamageHandler()
        {
            EventBus.Subscribe<OnAttackCasted>(OnAttackEventRaised);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                EventBus.Unsubscribe<OnAttackCasted>(OnAttackEventRaised);
            }

            disposed = true;
        }

        ~DamageHandler()
        {
            EventBus.Unsubscribe<OnAttackCasted>(OnAttackEventRaised);
            Dispose(false);
        }

        private void OnAttackEventRaised(OnAttackCasted cast)
        {
            CombatActor attacker = CombatActorContainer.GetActorByInstanceID(cast.attackerActorInstanceID);
            CombatActor target = CombatActorContainer.GetActorByInstanceID(cast.targetActorInstanceID);

            if (attacker == null || target == null || attacker.Info.ActorCamp == target.Info.ActorCamp)
                return;

            float rawDamage = (float)attacker.Info.Attack * cast.multiplier;
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