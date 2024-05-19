using UnityEngine;
using DG.Tweening;
using System.Collections;
using KahaGameCore.Common;

namespace ProjectMIL.UI
{
    public class BouncingObject : MonoBehaviour
    {
        [SerializeField] private float bounceForce = 0.3f;
        [SerializeField] private float moveHight = 40f;

        private void Awake()
        {
            GeneralCoroutineRunner.Instance.StartCoroutine(IEBounceLoop());
        }

        private IEnumerator IEBounceLoop()
        {
            while (true)
            {
                transform.DOScale(new Vector3(1f + bounceForce, 1f - bounceForce, 1f), 1f);
                transform.DOLocalMoveY(-moveHight * bounceForce, 1f);

                yield return new WaitForSeconds(1f);

                transform.DOScale(new Vector3(1f - bounceForce, 1f + bounceForce, 1f), 0.25f);
                transform.DOLocalMoveY(moveHight * 2f * bounceForce, 0.25f);

                yield return new WaitForSeconds(0.25f);

                transform.DOScale(new Vector3(1f + bounceForce, 1f - bounceForce, 1f), 0.25f);

                yield return new WaitForSeconds(0.25f);

                transform.DOScale(Vector3.one, 0.15f);
                transform.DOLocalMoveY(-moveHight * bounceForce * 0.5f, 0.5f);

                yield return new WaitForSeconds(0.15f);

                transform.DOScale(new Vector3(1f - bounceForce * 0.5f, 1f + bounceForce * 0.5f, 1f), 0.15f);

                yield return new WaitForSeconds(0.15f);

                transform.DOScale(new Vector3(1f + bounceForce * 0.5f, 1f - bounceForce * 0.5f, 1f), 0.25f);

                yield return new WaitForSeconds(0.25f);

                transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
                transform.DOLocalMoveY(0f, 0.5f);

                yield return new WaitForSeconds(1f);
            }
        }
    }
}