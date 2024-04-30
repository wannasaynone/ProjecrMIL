using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace ProjectMIL.UI
{
    public class InfoPopup : MonoBehaviour
    {
        [SerializeField] private GameObject infoPopupRoot;
        [SerializeField] private UnityEvent onEnabled; // for workaround with ParticleImage
        [SerializeField] private UnityEvent onInfoPopupEnded; // for workaround with ParticleImage

        private void OnEnable()
        {
            onEnabled?.Invoke();
            KahaGameCore.Common.GeneralCoroutineRunner.Instance.StartCoroutine(IEShowInfoPopup());
        }

        private System.Collections.IEnumerator IEShowInfoPopup()
        {
            infoPopupRoot.SetActive(true);
            infoPopupRoot.transform.localScale = Vector3.zero;
            infoPopupRoot.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.3f);

            infoPopupRoot.transform.DOShakeRotation(0.6f, new Vector3(0f, 0f, 10f), 10, 20f);

            yield return new WaitForSeconds(0.6f);

            infoPopupRoot.transform.DOScale(Vector3.one * 0.75f, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);

            onInfoPopupEnded?.Invoke();
            infoPopupRoot.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
        }
    }
}