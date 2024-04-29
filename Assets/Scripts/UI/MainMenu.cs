using UnityEngine;
using UnityEngine.UI;

namespace ProjectMIL.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject adventureRoot;
        [SerializeField] private Image adventureProgressBarFillImage;
        [SerializeField] private float fullAdventureProgressBarWidth = 700f;
        [SerializeField] private float adventureProgressBarFillSpeed = 100f;
        [SerializeField] private float shineSpeed = 1f;
        [SerializeField] private float shineEndWaitTime = 1f;

        private RectTransform adventureProgressBarFillRectTransform;
        private Material adventureProgressBarFillMaterialClone;

        public void Button_OnAdventureButtonPressed()
        {
            GameEvent.EventBus.Publish(new GameEvent.OnAdventureButtonPressed());
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress());
        }

        private System.Collections.IEnumerator IEShowAdventureProgress()
        {
            if (adventureProgressBarFillMaterialClone == null)
            {
                adventureProgressBarFillMaterialClone = new Material(adventureProgressBarFillImage.material);
                adventureProgressBarFillImage.material = adventureProgressBarFillMaterialClone;
                KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowShineEffect());
            }

            if (adventureProgressBarFillRectTransform == null)
            {
                adventureProgressBarFillRectTransform = adventureProgressBarFillImage.GetComponent<RectTransform>();
            }

            float currentWidth = 0f;
            adventureProgressBarFillRectTransform.sizeDelta = new Vector2(currentWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

            while (currentWidth < fullAdventureProgressBarWidth)
            {
                currentWidth += adventureProgressBarFillSpeed * Time.deltaTime;
                adventureProgressBarFillRectTransform.sizeDelta = new Vector2(currentWidth, adventureProgressBarFillRectTransform.sizeDelta.y);
                yield return null;
            }

            adventureProgressBarFillRectTransform.sizeDelta = new Vector2(fullAdventureProgressBarWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

            adventureRoot.SetActive(false);
        }

        private System.Collections.IEnumerator IEShowShineEffect()
        {
            float shineLocation = -0.5f;
            adventureProgressBarFillMaterialClone.SetFloat("_ShineLocation", shineLocation);

            while (true)
            {
                shineLocation += Time.deltaTime * shineSpeed;

                if (shineLocation > 1.5f)
                {
                    shineLocation = -0.5f;
                    yield return new WaitForSeconds(shineEndWaitTime);
                }

                adventureProgressBarFillMaterialClone.SetFloat("_ShineLocation", shineLocation);
                yield return null;
            }
        }
    }
}