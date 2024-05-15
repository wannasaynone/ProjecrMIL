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

        private OnExpValueUpdated onExpValueUpdatedTemp;

        ////////////////////////////////////////// Buttons //////////////////////////////////////////

        public void Button_OnAdventureButtonPressed()
        {
            EventBus.Publish(new OnAdventureButtonPressed());
        }

        ////////////////////////////////////////// Buttons //////////////////////////////////////////

        public override void Initial()
        {
            EventBus.Subscribe<OnPlayerInitialed>(OnPlayerInitialed);
            EventBus.Subscribe<OnExpValueUpdated>(OnExpValueUpdated);
            EventBus.Subscribe<OnAdventureEventResultPanelDisabled>(OnAdventureEventResultPanelDisabled);
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnPlayerInitialed(OnPlayerInitialed initialed)
        {
            levelText.text = initialed.level.ToString();
            expProgressBarFillSlider.value = (float)initialed.exp / (float)initialed.requireExp;
            expText.text = initialed.exp + " / " + initialed.requireExp;
        }

        private void OnExpValueUpdated(OnExpValueUpdated updated)
        {
            onExpValueUpdatedTemp = updated;
        }

        private void OnAdventureEventResultPanelDisabled(OnAdventureEventResultPanelDisabled disabled)
        {
            if (onExpValueUpdatedTemp != null)
            {
                levelText.text = onExpValueUpdatedTemp.level.ToString();
                expProgressBarFillSlider.value = (float)onExpValueUpdatedTemp.newValue / (float)onExpValueUpdatedTemp.requireExp;
                expText.text = onExpValueUpdatedTemp.newValue + " / " + onExpValueUpdatedTemp.requireExp;
                onExpValueUpdatedTemp = null;
            }
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            levelText.text = e.currentLevel.ToString();
            expProgressBarFillSlider.value = (float)e.currentExp / e.requireExp;
            expText.text = e.currentExp.ToString() + "/" + e.requireExp.ToString();
        }
    }
}