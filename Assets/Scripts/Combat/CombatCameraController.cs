using DG.Tweening;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatCameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private Transform shakeRoot;
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
            CombatActor playerActor = CombatActorContainer.GetAnyActorByCamp(CombatActor.ActorInfo.Camp.Player, false);
            if (isShaking || (playerActor != null && e.attackerActorInstanceID != playerActor.GetInstanceID()))
                return;

            if (playerActor != null)
            {
                CombatActor attackerActor = CombatActorContainer.GetActorByInstanceID(e.attackerActorInstanceID);
                if (attackerActor is BossActor)
                {
                    shakeRoot.DOShakePosition(0.5f, 0.75f, 10, 90f, false, true).OnComplete(() => isShaking = false);
                }
                else
                {
                    shakeRoot.DOShakePosition(0.5f, 0.25f, 10, 90f, false, true).OnComplete(() => isShaking = false);
                }

                isShaking = true;
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

            CombatActor playerActor = CombatActorContainer.GetAnyActorByCamp(CombatActor.ActorInfo.Camp.Player, true);
            if (playerActor == null)
            {
                return;
            }

            transform.transform.position = Vector3.MoveTowards(transform.transform.position, playerActor.transform.position + cameraOffset, 10f * Time.deltaTime);
        }

    }
}