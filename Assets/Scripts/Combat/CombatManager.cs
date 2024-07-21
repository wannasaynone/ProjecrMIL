
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
        [SerializeField] private CombatResultPanel winPanel;
        [SerializeField] private CombatResultPanel losePanel;
        [SerializeField] private SelectRuneMenu selectRuneMenu;

        private LevelBase currentLevel;

        public void CreateLevel(OnCombatStartCalled e)
        {
            combatCameraController.ResetAllCombatStageSetting();

            switch (e.levelType)
            {
                case OnCombatStartCalled.LevelType.Normal:
                    currentLevel = new NormalLevel(e.difficulty, enemyPrefab);
                    break;
                case OnCombatStartCalled.LevelType.Boss:
                    currentLevel = new BossLevel(e.difficulty, bossPrefab);
                    break;
                default:
                    Debug.LogError("Invalid level type: " + e.levelType.ToString());
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

        private void OnLevelEnded(bool isWin)
        {
            DisableCombatInput();
            if (isWin)
            {
                winPanel.ShowWith(OnPanelClosed);
            }
            else
            {
                losePanel.ShowWith(OnPanelClosed);
            }
        }

        private void DisableCombatInput()
        {
            inputButtonRoot.SetActive(false);
            attackCommandHintPanel.gameObject.SetActive(false);
            attackCommandHintPanel.StopListening();
        }

        private void OnPanelClosed()
        {
            currentLevel = null;
            selectRuneMenu.StartSelect(OnRuneSelected);
        }

        private void OnRuneSelected(int index)
        {
            EventBus.Publish(new OnCombatEndCalled());
        }

        public void EndCombat()
        {
            if (currentLevel != null)
            {
                currentLevel.ForceEnd();
                DisableCombatInput();
                currentLevel = null;
            }

            gameObject.SetActive(false);
        }
    }
}