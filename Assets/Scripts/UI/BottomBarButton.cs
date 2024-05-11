using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ProjectMIL.UI
{
    public class BottomBarButton : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMPro.TextMeshProUGUI buttonText;

        public void Enable(bool enable)
        {
            if (enable)
            {
                iconImage.transform.DOLocalMoveY(40f, 0.15f).SetEase(Ease.Linear);
                buttonText.transform.DOLocalMoveY(-50f, 0.15f).SetEase(Ease.Linear);
                buttonText.DOColor(new Color(1f, 1f, 1f, 1f), 0.115f);
            }
            else
            {
                iconImage.transform.DOLocalMoveY(10f, 0.15f).SetEase(Ease.Linear);
                buttonText.transform.DOLocalMoveY(-100f, 0.15f).SetEase(Ease.Linear);
                buttonText.DOColor(new Color(1f, 1f, 1f, 0f), 0.15f);
            }
        }
    }
}