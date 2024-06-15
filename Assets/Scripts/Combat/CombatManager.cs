
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private GameObject inputButtonRoot;
        [SerializeField] private CombatCameraController combatCameraController;
        [SerializeField] private AttackCommandHintPanel attackCommandHintPanel;
        [SerializeField] private CombatActor playerPrefab;
        [SerializeField] private CombatActor enemyPrefab;
        [SerializeField] private CombatActor bossPrefab;

        private LevelBase currentLevel;

        public void CreateLevel(OnCombatStartCalled e)
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

            currentLevel.Create(e, playerPrefab);
            inputButtonRoot.SetActive(false);
            gameObject.SetActive(true);
            attackCommandHintPanel.StartListening();
        }

        public void StartCurrentLevel()
        {
            currentLevel.Start(OnLevelEnded);
            inputButtonRoot.SetActive(true);
        }

        private void OnLevelEnded()
        {
            // TODO: Show result
            currentLevel = null;
            inputButtonRoot.SetActive(false);
            attackCommandHintPanel.gameObject.SetActive(false);
            attackCommandHintPanel.StopListening();
            EventBus.Publish(new OnCombatEndCalled());
        }

        public void EndCombat()
        {
            gameObject.SetActive(false);
        }
    }
}