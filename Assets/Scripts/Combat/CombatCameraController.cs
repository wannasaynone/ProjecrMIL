using DG.Tweening;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatCameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private Transform background01;
        [SerializeField] private Transform background02;


        private int currentMapIndex = 0;
        private bool isShaking = false;

        private void Awake()
        {
            EventBus.Subscribe<OnAnyActorGotHit>(OnGotHit);
        }

        public void ResetAllCombatStageSetting()
        {
            transform.position = new Vector3(0, 0, -10);
            background01.position = new Vector3(0, 0, 0);
            background02.position = new Vector3(7.34f, 0, 0);
            currentMapIndex = 0;
        }

        private void OnGotHit(OnAnyActorGotHit e)
        {
            CombatActor playerActor = CombatActorContainer.GetAnyActorByCamp(CombatActor.ActorInfo.Camp.Player);
            if (isShaking || (playerActor != null && e.attackerActorInstanceID != playerActor.GetInstanceID()))
                return;

            if (playerActor != null)
            {
                isShaking = true;
                transform.DOMove(playerActor.transform.position + cameraOffset, 0.1f).OnComplete(() =>
                {
                    transform.DOShakePosition(0.5f, 0.25f, 10, 90f, false, true).OnComplete(() => isShaking = false);
                });
            }
        }

        private void Update()
        {
            if (transform.position.x >= 7.34f * (currentMapIndex + 1))
            {
                switch (Mathf.Abs(currentMapIndex % 2))
                {
                    case 0:
                        background01.position = new Vector3(7.34f * (currentMapIndex + 2), 0, 0);
                        break;
                    case 1:
                        background02.position = new Vector3(7.34f * (currentMapIndex + 2), 0, 0);
                        break;
                }
                currentMapIndex++;
            }

            if (transform.position.x < 7.34f * currentMapIndex)
            {
                switch (Mathf.Abs(currentMapIndex % 2))
                {
                    case 0:
                        background02.position = new Vector3(7.34f * (currentMapIndex - 1), 0, 0);
                        break;
                    case 1:
                        background01.position = new Vector3(7.34f * (currentMapIndex - 1), 0, 0);
                        break;
                }
                currentMapIndex--;
            }

            CombatActor playerActor = CombatActorContainer.GetAnyActorByCamp(CombatActor.ActorInfo.Camp.Player);
            if (playerActor == null)
            {
                return;
            }

            if (isShaking)
            {
                return;
            }

            transform.transform.position = Vector3.MoveTowards(transform.transform.position, playerActor.transform.position + cameraOffset, 10f * Time.deltaTime);
        }

    }
}