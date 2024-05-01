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
        [Header("Adventure")]
        [SerializeField] private GameObject adventureRoot;
        [SerializeField] private GameObject adventurePanelRoot;
        [SerializeField] private Image adventureProgressBarFillImage;
        [SerializeField] private float fullAdventureProgressBarWidth = 700f;
        [SerializeField] private float adventureProgressBarFillSpeed = 100f;
        [SerializeField] private float shineSpeed = 1f;
        [SerializeField] private float shineEndWaitTime = 1f;

        private RectTransform adventureProgressBarFillRectTransform;
        private Material adventureProgressBarFillMaterialClone;

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
            EventBus.Subscribe<OnAdventureEventCreated>(OnAdventureEventCreated);
            EventBus.Subscribe<OnExpValueUpdated>(OnExpValueUpdated);
            EventBus.Subscribe<OnAdventureEventResultPanelDisabled>(OnAdventureEventResultPanelDisabled);
        }

        private void OnPlayerInitialed(OnPlayerInitialed initialed)
        {
            levelText.text = initialed.level.ToString();
            expProgressBarFillSlider.value = (float)initialed.exp / (float)initialed.requireExp;
            expText.text = initialed.exp + " / " + initialed.requireExp;
        }

        private void OnAdventureEventCreated(OnAdventureEventCreated created)
        {
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress());
        }

        private void OnExpValueUpdated(OnExpValueUpdated updated)
        {
            onExpValueUpdatedTemp = updated;
        }

        private void OnAdventureEventResultPanelDisabled(OnAdventureEventResultPanelDisabled disabled)
        {
            levelText.text = onExpValueUpdatedTemp.level.ToString();
            expProgressBarFillSlider.value = (float)onExpValueUpdatedTemp.newValue / (float)onExpValueUpdatedTemp.requireExp;
            expText.text = onExpValueUpdatedTemp.newValue + " / " + onExpValueUpdatedTemp.requireExp;
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

            adventurePanelRoot.transform.localScale = Vector3.zero;

            adventurePanelRoot.transform.DOScale(Vector3.one * 1.15f, 0.2f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.2f);

            adventurePanelRoot.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            float randomAdventureProgressBarFillSpeed = UnityEngine.Random.Range(adventureProgressBarFillSpeed * 1f, adventureProgressBarFillSpeed * 3f);
            while (currentWidth < fullAdventureProgressBarWidth)
            {
                currentWidth += randomAdventureProgressBarFillSpeed * Time.deltaTime;
                adventureProgressBarFillRectTransform.sizeDelta = new Vector2(currentWidth, adventureProgressBarFillRectTransform.sizeDelta.y);
                yield return null;
            }

            adventureProgressBarFillRectTransform.sizeDelta = new Vector2(fullAdventureProgressBarWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

            adventureRoot.SetActive(false);

            EventBus.Publish(new OnAdventureProgressBarAnimationEnded());
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