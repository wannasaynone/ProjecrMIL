using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using ProjectMIL.GameEvent;

namespace ProjectMIL.UI
{
    public class InfoPopup : UIBase
    {
        [SerializeField] private GameObject infoPopupRoot;
        [SerializeField] private UnityEvent onEnabled; // for workaround with ParticleImage
        [SerializeField] private UnityEvent onInfoPopupEnded; // for workaround with ParticleImage

        public override void Initial()
        {
            EventBus.Subscribe<OnAdventureProgressBarAnimationEnded>(OnAdventureProgressBarAnimationEnded);
        }

        private void OnAdventureProgressBarAnimationEnded(OnAdventureProgressBarAnimationEnded ended)
        {
            gameObject.SetActive(true);
            onEnabled?.Invoke();
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowInfoPopup());
        }

        private System.Collections.IEnumerator IEShowInfoPopup()
        {
            infoPopupRoot.SetActive(true);
            infoPopupRoot.transform.localScale = Vector3.zero;
            infoPopupRoot.transform.DOScale(Vector3.one * 1.3f, 0.15f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.15f);

            infoPopupRoot.transform.DOShakeRotation(0.3f, 20f, 10, 20f, true, ShakeRandomnessMode.Harmonic);
            infoPopupRoot.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.3f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.75f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            onInfoPopupEnded?.Invoke();
            infoPopupRoot.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
        }
    }
}