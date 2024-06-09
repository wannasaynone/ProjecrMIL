using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public abstract class LevelBase
    {
        protected readonly int difficulty;

        private DamageHandler damageHandler;
        private CombatActor clonedPlayerActor;

        public LevelBase(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public void Start(OnCombatStartCalled e, CombatActor playerPrefab)
        {
            clonedPlayerActor = Object.Instantiate(playerPrefab);
            clonedPlayerActor.transform.position = new Vector3(-2f, -1f, 0);
            clonedPlayerActor.Initialize(new CombatActor.ActorInfo(new CombatActor.ActorInfo.Templete
            {
                maxHP = e.maxHP,
                attack = e.attack,
                defense = e.defense,
                speed = e.speed,
                critical = e.critical,
                criticalResistance = e.criticalResistance,
                effectiveness = e.effectiveness,
                effectivenessResistance = e.effectivenessResistance,
                camp = CombatActor.ActorInfo.Camp.Player

            }));
            damageHandler = new DamageHandler();

            OnStarted();
        }

        public void End()
        {
            OnStartToEnd();

            clonedPlayerActor.Dispose();
            damageHandler.Dispose();
            System.GC.SuppressFinalize(damageHandler);
        }

        protected abstract void OnStarted();
        protected abstract void OnStartToEnd();
    }
}