using DG.Tweening;
using UnityEngine;

namespace ProjectMIL.UI
{
    public class BottomBar : UIBase
    {
        [SerializeField] private Transform selectFrameTransform;
        [SerializeField] private BottomBarButton[] buttons;
        [SerializeField] private BottomBarButton defaultButton;

        private int enabledButtonIndex = -1;

        public override void Initial()
        {
            EnableButton(defaultButton);
        }

        public void EnableButton(BottomBarButton selectedButton)
        {
            int selectedIndex = -1;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] == selectedButton)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (enabledButtonIndex == selectedIndex || selectedIndex == -1)
            {
                return;
            }

            if (enabledButtonIndex != -1)
            {
                buttons[enabledButtonIndex].Enable(false);
            }

            buttons[selectedIndex].Enable(true);
            enabledButtonIndex = selectedIndex;
            KahaGameCore.Common.TimerManager.Schedule(Time.deltaTime * 2f, delegate
            {
                selectFrameTransform.DOMoveX(buttons[selectedIndex].transform.position.x, 0.15f).SetEase(Ease.Linear);
            });
        }
    }
}