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
            CombatActor clonedEnemyActor = Object.Instantiate(enemyPrefab);
            clonedEnemyActor.Initialize(new CombatActor.ActorInfo(new CombatActor.ActorInfo.Templete
            {
                maxHP = 100 + 100 * difficulty,
                attack = 100 + 100 * difficulty,
                defense = 100 + 100 * difficulty,
                speed = 100 + 60 * difficulty,
                critical = 10 * (difficulty - 1),
                criticalResistance = 10 * (difficulty - 1),
                effectiveness = 10 * (difficulty - 1),
                effectivenessResistance = 10 * (difficulty - 1),
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