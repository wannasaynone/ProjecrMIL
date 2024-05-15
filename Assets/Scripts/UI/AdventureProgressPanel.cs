using DG.Tweening;
using ProjectMIL.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectMIL.UI
{
    public class AdventureProgressPanel : UIBase
    {
        [SerializeField] private GameObject adventureRoot;
        [SerializeField] private GameObject adventurePanelRoot;
        [SerializeField] private Image adventureProgressBarFillImage;
        [SerializeField] private float fullAdventureProgressBarWidth = 700f;
        [SerializeField] private float adventureProgressBarFillSpeed = 100f;
        [SerializeField] private float shineSpeed = 1f;
        [SerializeField] private float shineEndWaitTime = 1f;
        [Header("Predict")]
        [SerializeField] private Image characterImage;
        [SerializeField] private Image predictImage;
        [SerializeField] private GameObject chargeVisualRoot;

        private int referenceValue;

        private RectTransform adventureProgressBarFillRectTransform;
        private Material adventureProgressBarFillMaterialClone;

        public override void Initial()
        {
            EventBus.Subscribe<OnAdventureEventCreated_Exp>(OnAdventureEventCreated_Exp);
            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated_Gold);
        }

        private void OnAdventureEventCreated_Gold(OnAdventureEventCreated_Gold created)
        {
            referenceValue = created.addGold;
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress());
        }

        private void OnAdventureEventCreated_Exp(OnAdventureEventCreated_Exp created)
        {
            referenceValue = created.addExp;
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress());
        }

        private Color GetPredictImageColor()
        {
            return predictImage.color;
        }

        private void SetPredictImageColor(Color color)
        {
            predictImage.color = color;
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

            float randomAdventureProgressBarFillSpeed = Random.Range(adventureProgressBarFillSpeed * 1f, adventureProgressBarFillSpeed * 3f);
            float randomStartPredictAnimationTime = Random.Range(0f, 0.6f);

            bool isPredictAnimationShown = false;
            bool random = Random.Range(0, 100) <= 5;

            while (currentWidth < fullAdventureProgressBarWidth)
            {
                currentWidth += randomAdventureProgressBarFillSpeed * Time.deltaTime;
                adventureProgressBarFillRectTransform.sizeDelta = new Vector2(currentWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

                if (!isPredictAnimationShown && currentWidth >= fullAdventureProgressBarWidth * randomStartPredictAnimationTime && referenceValue >= 1000)
                {
                    yield return KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowPredictAnimation_Level2());
                    isPredictAnimationShown = true;
                    randomAdventureProgressBarFillSpeed = adventureProgressBarFillSpeed * 10f;
                }
                else if (!isPredictAnimationShown && currentWidth >= fullAdventureProgressBarWidth * randomStartPredictAnimationTime &&
                        ((referenceValue < 1000 && referenceValue >= 500) || random))
                {
                    yield return KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowPredictAnimation_Level1());
                    isPredictAnimationShown = true;
                    randomAdventureProgressBarFillSpeed = adventureProgressBarFillSpeed * 5f;
                }

                yield return null;
            }

            adventureProgressBarFillRectTransform.sizeDelta = new Vector2(fullAdventureProgressBarWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

            adventureRoot.SetActive(false);

            chargeVisualRoot.SetActive(false);
            characterImage.transform.localScale = Vector3.one;

            EventBus.Publish(new OnAdventureProgressBarAnimationEnded());
        }

        private System.Collections.IEnumerator IEShowPredictAnimation_Level1()
        {
            predictImage.transform.localScale = Vector3.zero;
            predictImage.color = Color.white;
            predictImage.gameObject.SetActive(true);

            predictImage.transform.DOScale(Vector3.one * 5.5f, 0.2f).SetEase(Ease.OutBack);
            DOTween.To(GetPredictImageColor, SetPredictImageColor, Color.red, 0.2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.2f);

            predictImage.transform.DOScale(Vector3.one * 4f, 0.1f).SetEase(Ease.Linear);

            // TODO: SFX

            yield return new WaitForSeconds(0.5f);

            predictImage.gameObject.SetActive(false);
        }

        private System.Collections.IEnumerator IEShowPredictAnimation_Level2()
        {
            predictImage.transform.localScale = Vector3.zero;
            predictImage.color = Color.white;
            predictImage.gameObject.SetActive(true);

            predictImage.transform.DOScale(Vector3.one * 5.5f, 0.2f).SetEase(Ease.OutBack);
            DOTween.To(GetPredictImageColor, SetPredictImageColor, Color.red, 0.2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.2f);

            predictImage.transform.DOScale(Vector3.one * 4f, 0.1f).SetEase(Ease.Linear);

            // TODO: SFX

            yield return new WaitForSeconds(0.5f);

            predictImage.gameObject.SetActive(false);
            chargeVisualRoot.SetActive(true);

            characterImage.transform.DOScale(Vector3.one * 1.5f, 2f).SetEase(Ease.Linear);
            characterImage.transform.DOShakePosition(2f, 25f, 10, 90f, false, true);

            yield return new WaitForSeconds(2f);

            chargeVisualRoot.SetActive(false);
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