using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class BossLevel : LevelBase
    {
        private readonly CombatActor bossActorPrefab;
        private BossActor clonedBossActor;

        public BossLevel(int difficulty, CombatActor bossActorPrefab) : base(difficulty)
        {
            this.bossActorPrefab = bossActorPrefab;
        }

        protected override void OnCreated()
        {
            int valueDiff = difficulty < 0 ? 0 : difficulty;
            clonedBossActor = Object.Instantiate(bossActorPrefab) as BossActor;
            clonedBossActor.Initialize(new CombatActor.ActorInfo(new CombatActor.ActorInfo.Templete
            {
                maxHP = 3000 + 3000 * valueDiff,
                attack = 100 + 100 * valueDiff,
                defense = 100 + 100 * valueDiff,
                speed = 0 + 60 * valueDiff,
                critical = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                criticalResistance = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                effectiveness = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                effectivenessResistance = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                camp = CombatActor.ActorInfo.Camp.Enemy
            }));
            clonedBossActor.transform.position = new Vector3(2f, -1f, 0);
            clonedBossActor.Pause();
        }

        protected override void OnStarted()
        {
            EventBus.Subscribe<OnAnyActorDied>(OnAnyActorDied);
            clonedBossActor.Resume();
        }

        private void OnAnyActorDied(OnAnyActorDied e)
        {
            if (e.actorInstanceID == clonedBossActor.GetInstanceID()
                || e.actorInstanceID == ClonedPlayerActor.GetInstanceID())
            {
                End();
            }
        }

        protected override void OnStartToEnd()
        {
            EventBus.Unsubscribe<OnAnyActorDied>(OnAnyActorDied);
            clonedBossActor.Dispose();
            clonedBossActor = null;
        }
    }
}
