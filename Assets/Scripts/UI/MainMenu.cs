using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ProjectMIL.GameEvent;

namespace ProjectMIL.UI
{
    public class MainMenu : UIBase
    {
        [Header("Level and Exp panel")]
        [SerializeField] private TMPro.TextMeshProUGUI levelText;
        [SerializeField] private TMPro.TextMeshProUGUI expText;
        [SerializeField] private Slider expProgressBarFillSlider;
        [SerializeField] private GameObject levelUpHintRoot;

        private OnGoldValueUpdated onGoldValueUpdatedTemp;

        ////////////////////////////////////////// Buttons //////////////////////////////////////////

        public void Button_OnAdventureButtonPressed()
        {
            EventBus.Publish(new OnAdventureButtonPressed());
        }

        ////////////////////////////////////////// Buttons //////////////////////////////////////////

        public override void Initialize(Utlity.ContextHandler contextHandler)
        {
            EventBus.Subscribe<OnPlayerValueUpdated>(OnPlayerValueUpdated);
            EventBus.Subscribe<OnGoldValueUpdated>(OnGoldValueUpdated);
            EventBus.Subscribe<OnAdventureEventResultPanelDisabled>(OnAdventureEventResultPanelDisabled);
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnGoldValueUpdated(OnGoldValueUpdated updated)
        {
            onGoldValueUpdatedTemp = updated;
        }

        private void OnPlayerValueUpdated(OnPlayerValueUpdated initialed)
        {
            levelText.text = initialed.level.ToString();
            expProgressBarFillSlider.value = (float)initialed.gold / (float)initialed.requireExp;
            expText.text = initialed.gold + " / " + initialed.requireExp;
            levelUpHintRoot.SetActive(initialed.gold >= initialed.requireExp);
        }

        private void OnAdventureEventResultPanelDisabled(OnAdventureEventResultPanelDisabled disabled)
        {
            if (onGoldValueUpdatedTemp != null)
            {
                levelText.text = onGoldValueUpdatedTemp.currentLevel.ToString();
                expProgressBarFillSlider.value = (float)onGoldValueUpdatedTemp.newValue / (float)onGoldValueUpdatedTemp.requireExp;
                expText.text = onGoldValueUpdatedTemp.newValue + " / " + onGoldValueUpdatedTemp.requireExp;
                levelUpHintRoot.SetActive(onGoldValueUpdatedTemp.newValue >= onGoldValueUpdatedTemp.requireExp);
                onGoldValueUpdatedTemp = null;
            }
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            levelText.text = e.currentLevel.ToString();
            expProgressBarFillSlider.value = (float)e.currentGold / e.requireExp;
            expText.text = e.currentGold.ToString() + "/" + e.requireExp.ToString();
            levelUpHintRoot.SetActive(e.currentGold >= e.requireExp);
        }

        public void Button_GuideToLevelUp()
        {
            EventBus.Publish(new OnForceButtomBarButtonEnable() { buttonIndex = 1 });
        }
    }
}