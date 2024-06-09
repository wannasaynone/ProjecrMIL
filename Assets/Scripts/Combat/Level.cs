using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class Level
    {
        private readonly int difficulty;
        private DamageHandler damageHandler;
        private CombatActor clonedPlayerActor;
        private readonly List<CombatActor> clonedEnemyActors = new List<CombatActor>();

        public Level(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public void Start(OnCombatStartCalled e, CombatActor playerPrefab, CombatActor enemyPrefab)
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

            int spawnCount = Random.Range(15, 31);
            Vector3 lastSpawnPosition = new Vector3(2f, -1f, 0);
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy(enemyPrefab);
                ((EnemyActor)clonedEnemyActors[i]).AddSortingGroupSortingOrder(i);
                clonedEnemyActors[i].transform.position = lastSpawnPosition;
                lastSpawnPosition.x += Random.Range(0.5f, 2.5f);
            }

            damageHandler = new DamageHandler();
        }

        private void SpawnEnemy(CombatActor enemyPrefab)
        {
            int valueDiff = difficulty < 0 ? 0 : difficulty;
            CombatActor clonedEnemyActor = Object.Instantiate(enemyPrefab);
            clonedEnemyActor.Initialize(new CombatActor.ActorInfo(new CombatActor.ActorInfo.Templete
            {
                maxHP = 100 + 50 * valueDiff,
                attack = 50 + 50 * valueDiff,
                defense = 50 + 50 * valueDiff,
                speed = 0 + 10 * valueDiff,
                critical = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                criticalResistance = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                effectiveness = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                effectivenessResistance = 10 * (valueDiff - 1) < 0 ? 0 : 10 * (valueDiff - 1),
                camp = CombatActor.ActorInfo.Camp.Enemy
            }));
            clonedEnemyActors.Add(clonedEnemyActor);
        }

        public void End()
        {
            clonedPlayerActor.Dispose();
            for (int i = 0; i < clonedEnemyActors.Count; i++)
            {
                clonedEnemyActors[i].Dispose();
            }
            damageHandler.Dispose();
            System.GC.SuppressFinalize(damageHandler);
        }
    }
}