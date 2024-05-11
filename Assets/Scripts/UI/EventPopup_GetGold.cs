using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using ProjectMIL.GameEvent;

namespace ProjectMIL.UI
{
    public class EventPopup_GetGold : UIBase
    {
        [SerializeField] private GameObject infoPopupRoot;
        [SerializeField] private TMPro.TextMeshProUGUI titleText;
        [SerializeField] private TMPro.TextMeshProUGUI descText;
        [SerializeField] private GameObject particleImageRoot;
        [SerializeField] private UnityEvent onEnabled; // for workaround with ParticleImage
        [SerializeField] private UnityEvent onPlayParticleCalled; // for workaround with ParticleImage

        private OnAdventureEventCreated_Gold nextCreatedAdvEvent;
        private Color megaWinColor = Color.white;
        private float megaWinFontSize = 0f;
        private int curMegaWinNumber = 0;

        public override void Initial()
        {
            EventBus.Subscribe<OnAdventureProgressBarAnimationEnded>(OnAdventureProgressBarAnimationEnded);
            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated);
        }

        private void OnDisable()
        {
            nextCreatedAdvEvent = null;
            EventBus.Publish(new OnAdventureEventResultPanelDisabled());
        }

        private void OnAdventureProgressBarAnimationEnded(OnAdventureProgressBarAnimationEnded ended)
        {
            if (nextCreatedAdvEvent == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(nextCreatedAdvEvent.title))
            {
                nextCreatedAdvEvent.title = "MISSING TITLE";
            }

            if (string.IsNullOrEmpty(nextCreatedAdvEvent.description))
            {
                nextCreatedAdvEvent.description = "MISSING DESCRIPTION";
            }

            titleText.text = nextCreatedAdvEvent.title;

            gameObject.SetActive(true);
            infoPopupRoot.SetActive(false);

            particleImageRoot.SetActive(false);
            onEnabled?.Invoke();

            if (nextCreatedAdvEvent.addGold < 100)
            {
                KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowInfoPopup_Level1());
                descText.text = string.Format(nextCreatedAdvEvent.description, nextCreatedAdvEvent.addGold);
            }
            else if (nextCreatedAdvEvent.addGold >= 100 && nextCreatedAdvEvent.addGold < 500)
            {
                KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowInfoPopup_Level2());
                descText.text = string.Format(nextCreatedAdvEvent.description, "<size=" + descText.fontSize * 1.5f + ">" + nextCreatedAdvEvent.addGold + "</size>");
            }
            else if (nextCreatedAdvEvent.addGold >= 500 && nextCreatedAdvEvent.addGold < 1000)
            {
                KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowInfoPopup_Level3());
                descText.text = string.Format(nextCreatedAdvEvent.description, nextCreatedAdvEvent.addGold);
            }
            else
            {
                KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowInfoPopup_Level4());
                descText.text = string.Format(nextCreatedAdvEvent.description, nextCreatedAdvEvent.addGold);
            }
        }

        private void OnAdventureEventCreated(OnAdventureEventCreated_Gold onAdventureEventCreated)
        {
            nextCreatedAdvEvent = onAdventureEventCreated;
        }

        private System.Collections.IEnumerator IEShowInfoPopup_Level1()
        {
            infoPopupRoot.SetActive(true);
            infoPopupRoot.transform.localScale = Vector3.zero;
            infoPopupRoot.transform.DOScale(Vector3.one * 1.1f, 0.15f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.15f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        }

        private System.Collections.IEnumerator IEShowInfoPopup_Level2()
        {
            infoPopupRoot.SetActive(true);
            infoPopupRoot.transform.localScale = Vector3.zero;
            infoPopupRoot.transform.DOScale(Vector3.one * 1.25f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.75f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one * 1.15f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.85f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one * 1.05f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.95f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        }

        private System.Collections.IEnumerator IEShowInfoPopup_Level3()
        {
            infoPopupRoot.SetActive(true);
            infoPopupRoot.transform.localScale = Vector3.zero;
            infoPopupRoot.transform.DOScale(Vector3.one * 1.3f, 0.15f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.15f);

            infoPopupRoot.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase(Ease.Linear);

            megaWinFontSize = titleText.fontSize;
            DOTween.To(() => GetCurrentFontSize(), x => SetCurrentFontSize(x), titleText.fontSize * 2f, 1f);

            curMegaWinNumber = 0;
            DOTween.To(() => GetCurrentMegaWinNumber(), x => SetCurrentMegaWinNumber(x), nextCreatedAdvEvent.addGold, 1f).OnUpdate(() =>
            {
                descText.text = string.Format(nextCreatedAdvEvent.description, "<size=" + megaWinFontSize + ">" + curMegaWinNumber + "</size>");
            });

            yield return new WaitForSeconds(1f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.75f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            infoPopupRoot.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
        }

        private System.Collections.IEnumerator IEShowInfoPopup_Level4()
        {
            infoPopupRoot.SetActive(true);
            infoPopupRoot.transform.localScale = Vector3.zero;
            infoPopupRoot.transform.DOScale(Vector3.one * 1.3f, 0.15f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.15f);

            infoPopupRoot.transform.DOShakeRotation(1.5f, 20f, 10, 20f, true, ShakeRandomnessMode.Harmonic);
            infoPopupRoot.transform.DOScale(Vector3.one * 1.75f, 1.5f).SetEase(Ease.Linear);

            megaWinFontSize = titleText.fontSize;
            DOTween.To(() => GetCurrentFontSize(), x => SetCurrentFontSize(x), titleText.fontSize * 2f, 1.5f);

            megaWinColor = Color.white;
            DOTween.To(() => GetCurrentColor(), x => SetCurrentColor(x), Color.yellow, 1.5f);

            curMegaWinNumber = 0;
            DOTween.To(() => GetCurrentMegaWinNumber(), x => SetCurrentMegaWinNumber(x), nextCreatedAdvEvent.addGold, 1.5f).OnUpdate(() =>
            {
                descText.text = string.Format(nextCreatedAdvEvent.description, "<size=" + megaWinFontSize + "><color=#" + ColorUtility.ToHtmlStringRGB(megaWinColor) + ">" + curMegaWinNumber + "</size></color>");
            });

            yield return new WaitForSeconds(1.5f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.75f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            particleImageRoot.SetActive(true);
            onPlayParticleCalled?.Invoke();
            infoPopupRoot.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
        }

        private Color GetCurrentColor()
        {
            return megaWinColor;
        }

        private void SetCurrentColor(Color color)
        {
            megaWinColor = color;
        }

        private float GetCurrentFontSize()
        {
            return megaWinFontSize;
        }

        private void SetCurrentFontSize(float fontSize)
        {
            megaWinFontSize = fontSize;
        }

        private int GetCurrentMegaWinNumber()
        {
            return curMegaWinNumber;
        }

        private void SetCurrentMegaWinNumber(int number)
        {
            curMegaWinNumber = number;
        }
    }
}