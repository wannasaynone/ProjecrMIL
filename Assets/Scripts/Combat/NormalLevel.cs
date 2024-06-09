using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class NormalLevel : LevelBase
    {
        private readonly CombatActor enemyPrefab;
        private readonly List<CombatActor> clonedEnemyActors = new List<CombatActor>();

        public NormalLevel(int difficulty, CombatActor enemyPrefab) : base(difficulty)
        {
            this.enemyPrefab = enemyPrefab;
        }

        protected override void OnStarted()
        {
            int spawnCount = Random.Range(15, 31);
            Vector3 lastSpawnPosition = new Vector3(2f, -1f, 0);
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy(enemyPrefab);
                ((EnemyActor)clonedEnemyActors[i]).AddSortingGroupSortingOrder(i);
                clonedEnemyActors[i].transform.position = lastSpawnPosition;
                lastSpawnPosition.x += Random.Range(0.5f, 2.5f);
            }
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

        protected override void OnStartToEnd()
        {
            for (int i = 0; i < clonedEnemyActors.Count; i++)
            {
                clonedEnemyActors[i].Dispose();
            }
        }
    }
}