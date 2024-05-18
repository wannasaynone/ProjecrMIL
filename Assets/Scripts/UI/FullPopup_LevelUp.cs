using ProjectMIL.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectMIL.UI
{
    public class FullPopup_LevelUp : UIBase
    {
        [SerializeField] private Transform levelBadgeRoot;
        [SerializeField] private TMPro.TextMeshProUGUI levelText;
        [SerializeField] private Transform backLightRoot;
        [SerializeField] private Transform[] starRoots;
        [SerializeField] private GameObject levelUpRewardRoot;
        [SerializeField] private GameObject tapToContinueRoot;

        public override void Initial()
        {
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            gameObject.SetActive(true);
            backLightRoot.gameObject.SetActive(false);
            for (int i = 0; i < starRoots.Length; i++)
            {
                starRoots[i].gameObject.SetActive(false);
            }
            levelUpRewardRoot.SetActive(false);
            tapToContinueRoot.SetActive(false);

            levelText.text = e.oldLevel.ToString();

            Debug.Log("Level Up " + e.oldLevel + " -> " + e.currentLevel);
        }
    }
}