using System.Collections;
using KahaGameCore.Common;
using ProjectMIL.GameEvent;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ProjectMIL.UI
{
    public class FullPopup_LevelUp : UIBase
    {
        [SerializeField] private Transform levelBadgeRoot;
        [SerializeField] private TMPro.TextMeshProUGUI levelText;
        [SerializeField] private Transform backLightRoot;
        [SerializeField] private Transform[] starRoots;
        [SerializeField] private CanvasGroup addStatsPanel;
        [SerializeField] private GameObject tapToContinueRoot;
        [SerializeField] private UnityEvent onEnabled; // for workaround with ParticleImage
        [SerializeField] private UnityEvent onParticleCalled; // for workaround with ParticleImage
        [SerializeField] private Button rootButton;

        [System.Serializable]
        private class AddStatsGroup
        {
            public string statsName;
            public TMPro.TextMeshProUGUI beforeText;
            public TMPro.TextMeshProUGUI afterText;
            public GameObject arrowRoot;
        }

        [SerializeField] private AddStatsGroup[] addStatsGroups;

        private OnLevelUpdated tempEvent;

        public override void Initialize()
        {
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            onEnabled.Invoke();
            rootButton.interactable = false;
            gameObject.SetActive(true);
            backLightRoot.gameObject.SetActive(false);
            for (int i = 0; i < starRoots.Length; i++)
            {
                starRoots[i].gameObject.SetActive(false);
            }
            addStatsPanel.gameObject.SetActive(false);
            tapToContinueRoot.SetActive(false);

            levelText.text = e.oldLevel.ToString();

            for (int i = 0; i < addStatsGroups.Length; i++)
            {
                switch (addStatsGroups[i].statsName)
                {
                    case "MaxHP":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeHP, e.afterHP);
                        break;
                    case "Defense":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeDefense, e.afterDefense);
                        break;
                    case "Attack":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeAttack, e.afterAttack);
                        break;
                    case "Speed":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeSpeed, e.afterSpeed);
                        break;
                    case "Critical":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeCritical, e.afterCritical);
                        break;
                    case "CriticalResistance":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeCriticalResistance, e.afterCriticalResistance);
                        break;
                    case "Effectiveness":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeEffectiveness, e.afterEffectiveness);
                        break;
                    case "EffectivenessResistance":
                        SetUpAddStatsObject(addStatsGroups[i], e.beforeEffectivenessResistance, e.afterEffectivenessResistance);
                        break;
                }
            }

            tempEvent = e;
            GeneralCoroutineRunner.Instance.StartCoroutine(IEShowLevelUp());
        }

        private void SetUpAddStatsObject(AddStatsGroup addStatsGroup, int beforeValue, int afterValue)
        {
            addStatsGroup.beforeText.text = beforeValue.ToString();
            addStatsGroup.afterText.text = afterValue > beforeValue ? afterValue.ToString() : "";
            addStatsGroup.arrowRoot.SetActive(afterValue > beforeValue);
        }

        private IEnumerator IEShowLevelUp()
        {
            levelBadgeRoot.DOScale(Vector3.one * 1.25f, 0.15f);
            levelText.transform.DOScale(Vector3.one * 1.25f, 0.15f);
            yield return new WaitForSeconds(0.15f);
            levelBadgeRoot.DOScale(Vector3.one, 0.15f);
            levelText.transform.DOScale(Vector3.one, 0.15f);
            yield return new WaitForSeconds(0.15f);
            levelBadgeRoot.DOScale(Vector3.one * 0.5f, 1f);
            levelText.transform.DOScale(Vector3.one * 0.5f, 1f);
            levelBadgeRoot.transform.DOShakeRotation(1f, 20f, 10, 20f, true, ShakeRandomnessMode.Harmonic);
            yield return new WaitForSeconds(1f);
            levelText.text = tempEvent.currentLevel.ToString();
            levelText.transform.DOScale(Vector3.one * 3f, 0.15f);
            yield return new WaitForSeconds(0.15f);
            levelBadgeRoot.DOScale(Vector3.one * 3f, 0.15f);
            yield return new WaitForSeconds(0.15f);
            levelBadgeRoot.DOScale(Vector3.one * 3.25f, 0.5f);
            levelText.transform.DOScale(Vector3.one * 3.25f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            levelText.transform.DOScale(Vector3.one, 0.05f);
            levelBadgeRoot.DOScale(Vector3.one, 0.05f);
            yield return new WaitForSeconds(0.05f);
            backLightRoot.gameObject.SetActive(true);
            backLightRoot.transform.localScale = Vector3.zero;
            backLightRoot.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
            for (int i = 0; i < starRoots.Length; i++)
            {
                starRoots[i].gameObject.SetActive(true);
                starRoots[i].localScale = Vector3.zero;
                starRoots[i].DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
            }
            levelText.transform.DOScale(Vector3.one * 0.85f, 0.15f);
            levelBadgeRoot.DOScale(Vector3.one * 0.85f, 0.15f);
            onParticleCalled.Invoke();
            yield return new WaitForSeconds(0.15f);
            GeneralCoroutineRunner.Instance.StartCoroutine(IEDoLightEndlessScaleAnimation());
            GeneralCoroutineRunner.Instance.StartCoroutine(IEDoStarEndlessPositionAnimation());
            levelText.transform.DOScale(Vector3.one, 0.15f);
            levelBadgeRoot.DOScale(Vector3.one, 0.15f);
            addStatsPanel.alpha = 0f;
            addStatsPanel.gameObject.SetActive(true);
            addStatsPanel.transform.localPosition = new Vector3(0f, 140f, 0f);

            addStatsPanel.DOFade(1f, 0.1f);
            addStatsPanel.transform.DOLocalMoveY(-70f, 0.1f).SetEase(Ease.Linear);

            tapToContinueRoot.transform.localScale = Vector3.zero;
            tapToContinueRoot.SetActive(true);
            tapToContinueRoot.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
            rootButton.interactable = true;
        }

        private IEnumerator IEDoLightEndlessScaleAnimation()
        {
            while (true)
            {
                if (!backLightRoot.gameObject.activeSelf)
                {
                    yield break;
                }
                backLightRoot.DOScale(Vector3.one * 1.7f, 1f).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1.25f);
                if (!backLightRoot.gameObject.activeSelf)
                {
                    yield break;
                }
                backLightRoot.DOScale(Vector3.one, 1f).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1f);
            }
        }

        private List<Vector3> starOriginalPos = new List<Vector3>();
        private IEnumerator IEDoStarEndlessPositionAnimation()
        {
            if (starOriginalPos.Count == 0)
            {
                for (int i = 0; i < starRoots.Length; i++)
                {
                    starOriginalPos.Add(starRoots[i].localPosition);
                }
            }
            while (true)
            {
                if (!starRoots[0].gameObject.activeSelf)
                {
                    yield break;
                }
                for (int i = 0; i < starRoots.Length; i++)
                {
                    starRoots[i].DOLocalMove(starOriginalPos[i] + Vector3.right * Random.Range(-10f, 10f) + Vector3.up * Random.Range(-10f, 10f), 2f).SetEase(Ease.Linear);
                    starRoots[i].DOScale(Vector3.one * 1.1f, 2f).SetEase(Ease.Linear);
                    starRoots[i].DORotate(Vector3.forward * Random.Range(-10f, 10f), 2f).SetEase(Ease.Linear);
                }
                yield return new WaitForSeconds(2f);
                if (!starRoots[0].gameObject.activeSelf)
                {
                    yield break;
                }
                for (int i = 0; i < starRoots.Length; i++)
                {
                    starRoots[i].DOLocalMove(starOriginalPos[i], 1f).SetEase(Ease.Linear);
                    starRoots[i].DOScale(Vector3.one, 1f).SetEase(Ease.Linear);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}