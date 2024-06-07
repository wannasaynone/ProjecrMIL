using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace ProjectMIL.Combat
{
    [RequireComponent(typeof(TMPro.TextMeshPro))]
    public class DamageNumberObject : MonoBehaviour
    {
        private TMPro.TextMeshPro textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TMPro.TextMeshPro>();
            StartCoroutine(IEShowAnimation());
        }

        private IEnumerator IEShowAnimation()
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