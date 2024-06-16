using System;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public abstract class LevelBase
    {
        protected readonly int difficulty;

        private DamageHandler damageHandler;
        protected CombatActor ClonedPlayerActor { get; private set; }
        private Action<bool> onEnded;

        public LevelBase(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public void Create(OnCombatStartCalled e, CombatActor playerPrefab)
        {
            ClonedPlayerActor = UnityEngine.Object.Instantiate(playerPrefab);
            ClonedPlayerActor.transform.position = new Vector3(-2f, -1f, 0);
            ClonedPlayerActor.Initialize(new CombatActor.ActorInfo(new CombatActor.ActorInfo.Templete
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

            OnCreated();
        }

        public void Start(Action<bool> onEnded)
        {
            this.onEnded = onEnded;
            OnStarted();
        }

        protected void End()
        {
            OnStartToEnd();

            bool isWin = ClonedPlayerActor.Info.currentHP > 0;
            ClonedPlayerActor.Dispose();
            damageHandler.Dispose();
            GC.SuppressFinalize(damageHandler);

            onEnded?.Invoke(isWin);
        }

        protected abstract void OnStarted();
        protected abstract void OnCreated();
        protected abstract void OnStartToEnd();
    }
}