
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
        [SerializeField] private CombatActor bossPrefab;

        private LevelBase currentLevel;

        public void StartCombat(OnCombatStartCalled e)
        {
            combatCameraController.ResetAllCombatStageSetting();

            switch (e.levelType)
            {
                case 0:
                    currentLevel = new NormalLevel(e.difficulty, enemyPrefab);
                    break;
                case 1:
                    currentLevel = new BossLevel(e.difficulty, bossPrefab);
                    break;
                default:
                    Debug.LogError("Invalid level type: " + e.levelType);
                    return;
            }

            currentLevel.Start(e, playerPrefab);
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