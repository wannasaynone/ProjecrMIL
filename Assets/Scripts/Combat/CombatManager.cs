
using DG.Tweening;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private CombatCameraController combatCameraController;
        [SerializeField] private CombatActor playerPrefab;
        [SerializeField] private CombatActor enemyPrefab;

        private Level currentLevel;


        public void StartCombat(OnCombatStartCalled e)
        {
            combatCameraController.ResetAllCombatStageSetting();
            currentLevel = new Level(0); // TODO: handle difficulty
            currentLevel.Start(e, playerPrefab, enemyPrefab);
            gameObject.SetActive(true);
        }

        public void EndCombat()
        {
            currentLevel.End();
            gameObject.SetActive(false);
        }
    }
}