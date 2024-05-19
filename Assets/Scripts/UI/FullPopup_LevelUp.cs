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
        [SerializeField] private GameObject levelUpRewardRoot;
        [SerializeField] private GameObject tapToContinueRoot;
        [SerializeField] private UnityEvent onParticleCalled; // for workaround with ParticleImage
        [SerializeField] private Button rootButton;

        private OnLevelUpdated tempEvent;

        public override void Initial()
        {
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            rootButton.interactable = false;
            gameObject.SetActive(true);
            backLightRoot.gameObject.SetActive(false);
            for (int i = 0; i < starRoots.Length; i++)
            {
                starRoots[i].gameObject.SetActive(false);
            }
            levelUpRewardRoot.SetActive(false);
            tapToContinueRoot.SetActive(false);

            levelText.text = e.oldLevel.ToString();

            tempEvent = e;
            GeneralCoroutineRunner.Instance.StartCoroutine(IEShowLevelUp());
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
            levelUpRewardRoot.transform.localScale = Vector3.zero;
            levelUpRewardRoot.SetActive(true);
            levelUpRewardRoot.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
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