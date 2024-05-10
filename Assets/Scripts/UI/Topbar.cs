using ProjectMIL.GameEvent;

namespace ProjectMIL.UI
{
    public class Topbar : UIBase
    {
        [UnityEngine.SerializeField] private TMPro.TextMeshProUGUI goldText;

        private OnGoldValueUpdated onGoldValueUpdatedTemp;

        public override void Initial()
        {
            EventBus.Subscribe<OnPlayerInitialed>(OnPlayerInitialed);
            EventBus.Subscribe<OnGoldValueUpdated>(OnGoldValueUpdated);
            EventBus.Subscribe<OnAdventureEventResultPanelDisabled>(OnAdventureEventResultPanelDisabled);
        }

        private void OnPlayerInitialed(OnPlayerInitialed initialed)
        {
            goldText.text = initialed.gold.ToString();
        }

        private void OnGoldValueUpdated(OnGoldValueUpdated updated)
        {
            onGoldValueUpdatedTemp = updated;
        }

        private void OnAdventureEventResultPanelDisabled(OnAdventureEventResultPanelDisabled disabled)
        {
            if (onGoldValueUpdatedTemp != null)
            {
                goldText.text = onGoldValueUpdatedTemp.newValue.ToString();
                onGoldValueUpdatedTemp = null;
            }
        }
    }
}