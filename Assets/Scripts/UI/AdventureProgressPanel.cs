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
        [SerializeField] private RectTransform enemyRoot;
        [SerializeField] private ParticleSystem enemyLandingEffect;

        private int referenceValue;
        private float currentProgressBarWidth = 0f;
        private float randomAdventureProgressBarFillSpeed;
        private float randomStartPredictAnimationTime;

        private RectTransform adventureProgressBarFillRectTransform;
        private Material adventureProgressBarFillMaterialClone;

        public override void Initialize()
        {
            EventBus.Subscribe<OnAdventureEventCreated_Exp>(OnAdventureEventCreated_Exp);
            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated_Gold);
            EventBus.Subscribe<OnAdventureEventCreated_EncounterEnemy>(OnAdventureEventCreated_EncounterEnemy);
            EventBus.Subscribe<OnAdventureEventCreated_EncounterBoss>(OnAdventureEventCreated_EncounterBoss);
        }

        private void OnAdventureEventCreated_EncounterEnemy(OnAdventureEventCreated_EncounterEnemy created)
        {
            referenceValue = created.difficulty;
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress_EncounterEnemy());
        }

        private void OnAdventureEventCreated_EncounterBoss(OnAdventureEventCreated_EncounterBoss created)
        {
            referenceValue = created.difficulty;
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress_EncounterEnemy()); // TODO: Change to EncounterBoss
        }

        private void OnAdventureEventCreated_Gold(OnAdventureEventCreated_Gold created)
        {
            referenceValue = created.addGold;
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress_Normal());
        }

        private void OnAdventureEventCreated_Exp(OnAdventureEventCreated_Exp created)
        {
            referenceValue = created.addExp;
            adventureRoot.SetActive(true);
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowAdventureProgress_Normal());
        }

        private Color GetPredictImageColor()
        {
            return predictImage.color;
        }

        private void SetPredictImageColor(Color color)
        {
            predictImage.color = color;
        }

        private System.Collections.IEnumerator InitializeAdventureProgressPanel()
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

            enemyRoot.gameObject.SetActive(false);
            enemyRoot.anchoredPosition = new Vector3(325f, -7.5f, 0f);
            currentProgressBarWidth = 0f;
            adventureProgressBarFillRectTransform.sizeDelta = new Vector2(currentProgressBarWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

            adventurePanelRoot.transform.localScale = Vector3.zero;

            adventurePanelRoot.transform.DOScale(Vector3.one * 1.15f, 0.2f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.2f);

            adventurePanelRoot.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            randomAdventureProgressBarFillSpeed = Random.Range(adventureProgressBarFillSpeed * 1f, adventureProgressBarFillSpeed * 3f);
            randomStartPredictAnimationTime = Random.Range(0f, 0.6f);
        }

        private System.Collections.IEnumerator IEShowAdventureProgress_EncounterEnemy()
        {
            yield return InitializeAdventureProgressPanel();
            yield return IERunProgressBar(IEOnReachShowPredictProgressBarWidth_EncounterEnemy());
            EndAnimation();
        }

        private System.Collections.IEnumerator IEShowAdventureProgress_Normal()
        {
            yield return InitializeAdventureProgressPanel();
            yield return IERunProgressBar(IEOnReachShowPredictProgressBarWidth_ExpAndGold());
            EndAnimation();
        }

        private System.Collections.IEnumerator IEOnReachShowPredictProgressBarWidth_ExpAndGold()
        {
            bool random = Random.Range(0, 100) <= 5;

            if (referenceValue >= 1000)
            {
                yield return IEShowPredictAnimation_Level2();
                randomAdventureProgressBarFillSpeed = adventureProgressBarFillSpeed * 10f;
            }
            else if ((referenceValue < 1000 && referenceValue >= 500) || random)
            {
                yield return IEShowPredictAnimation_Level1();
                randomAdventureProgressBarFillSpeed = adventureProgressBarFillSpeed * 5f;
            }
        }

        private System.Collections.IEnumerator IEOnReachShowPredictProgressBarWidth_EncounterEnemy()
        {
            yield return IEShowPredictAnimation_Level1();

            enemyRoot.transform.localScale = Vector3.zero;
            enemyRoot.DOLocalMoveY(90f, 0.1f);
            enemyRoot.DOScale(Vector3.one * 1.2f, 0.1f);
            enemyRoot.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            enemyRoot.DOLocalMoveY(100f, 0.2f);

            yield return new WaitForSeconds(0.2f);

            enemyRoot.transform.DOLocalMoveY(12.5f, 0.1f);
            enemyRoot.DOScale(Vector3.one, 0.1f);

            yield return new WaitForSeconds(0.1f);

            enemyLandingEffect.Play();

            yield return new WaitForSeconds(0.5f);

            enemyRoot.DOJump((characterImage.transform.position + enemyRoot.position) / 2f, 0.5f, 1, 0.25f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);

            enemyRoot.DOJump(characterImage.transform.position, 1.5f, 1, 0.35f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.35f);

            currentProgressBarWidth = fullAdventureProgressBarWidth;
        }

        private System.Collections.IEnumerator IERunProgressBar(System.Collections.IEnumerator OnReachShowPredictProgressBarWidth)
        {
            bool predictReached = false;

            while (currentProgressBarWidth < fullAdventureProgressBarWidth)
            {
                currentProgressBarWidth += randomAdventureProgressBarFillSpeed * Time.deltaTime;
                adventureProgressBarFillRectTransform.sizeDelta = new Vector2(currentProgressBarWidth, adventureProgressBarFillRectTransform.sizeDelta.y);

                if (currentProgressBarWidth >= fullAdventureProgressBarWidth * randomStartPredictAnimationTime)
                {
                    if (!predictReached)
                    {
                        predictReached = true;
                        yield return OnReachShowPredictProgressBarWidth;
                    }
                }

                yield return null;
            }
        }

        private void EndAnimation()
        {
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