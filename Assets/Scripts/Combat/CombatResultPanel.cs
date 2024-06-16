using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatResultPanel : MonoBehaviour
    {
        private System.Action onPanelClosed;

        public void ShowWith(System.Action onPanelClosed)
        {
            gameObject.SetActive(true);
            this.onPanelClosed = onPanelClosed;
        }

        public void Button_Close()
        {
            gameObject.SetActive(false);
            onPanelClosed?.Invoke();
        }
    }
}