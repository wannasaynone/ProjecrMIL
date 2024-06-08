using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace ProjectMIL.Combat
{
    [RequireComponent(typeof(TMPro.TextMeshPro))]
    public class DamageNumberObject : MonoBehaviour
    {
        public enum AnimationType
        {
            UpFade,
            Fall
        }

        private TMPro.TextMeshPro textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TMPro.TextMeshPro>();
        }

        public void ShowAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.UpFade:
                    StartCoroutine(IEShowAnimation_Up());
                    break;
                case AnimationType.Fall:
                    StartCoroutine(IEShowAnimation_Fall());
                    break;
            }
        }

        private IEnumerator IEShowAnimation_Up()
        {
            transform.DOScale(2.5f, 0.1f);
            yield return new WaitForSeconds(0.1f);
            transform.DOScale(1f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            transform.DOMoveY(transform.position.y + 1f, 0.5f);
            DOTween.To(GetAlpha, SetAlpha, 0f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }

        private IEnumerator IEShowAnimation_Fall()
        {
            transform.DOJump(transform.position - Vector3.up * 1.5f + Vector3.right * Random.Range(-0.5f, 0.5f) - Vector3.forward * 5f, 3f, 1, 1f);
            yield return new WaitForSeconds(0.5f);
            DOTween.To(GetAlpha, SetAlpha, 0f, 0.5f);
            transform.DOScale(0f, 0.5f);
        }

        private void SetAlpha(float alpha)
        {
            Color color = textMeshPro.color;
            color.a = alpha;
            textMeshPro.color = color;
        }

        private float GetAlpha()
        {
            return textMeshPro.color.a;
        }

        public void SetDamage(float damage)
        {
            textMeshPro.text = damage.ToString();
        }

        public void SetText(string text)
        {
            textMeshPro.text = text;
        }
    }
}