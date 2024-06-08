
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private CombatCameraController combatCameraController;
        [SerializeField] private AttackCommandHintPanel attackCommandHintPanel;
        [SerializeField] private CombatActor playerPrefab;
        [SerializeField] private CombatActor enemyPrefab;

        private Level currentLevel;

        public void StartCombat(OnCombatStartCalled e)
        {
            combatCameraController.ResetAllCombatStageSetting();
            currentLevel = new Level(e.difficulty);
            currentLevel.Start(e, playerPrefab, enemyPrefab);
            gameObject.SetActive(true);
            attackCommandHintPanel.StartListening();
        }

        public void EndCombat()
        {
            currentLevel.End();
            gameObject.SetActive(false);
            attackCommandHintPanel.gameObject.SetActive(false);
            attackCommandHintPanel.StopListening();
        }
    }
}