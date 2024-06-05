
using DG.Tweening;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private Transform background01;
        [SerializeField] private Transform background02;
        [SerializeField] private CombatActor playerPrefab;
        [SerializeField] private CombatActor enemyPrefab;

        private int currentMapIndex = 0;
        private bool isShaking = false;
        private Level currentLevel;

        private void Awake()
        {
            EventBus.Subscribe<OnGotHit>(OnGotHit);
        }

        private void OnGotHit(OnGotHit e)
        {
            if (isShaking || CombatActorContainer.GetActorByInstanceID(e.targetActorInstanceID).Info.ActorCamp == CombatActor.ActorInfo.Camp.Player)
                return;

            isShaking = true;
            CombatActor playerActor = CombatActorContainer.GetAnyUnitByCamp(CombatActor.ActorInfo.Camp.Player);
            cameraRoot.DOMove(playerActor.transform.position + cameraOffset, 0.1f).OnComplete(() =>
            {
                cameraRoot.DOShakePosition(0.5f, 0.25f, 10, 90f, false, true).OnComplete(() => isShaking = false);
            });
        }

        public void StartCombat(OnCombatStartCalled e)
        {
            ResetAllCombatStageSetting();
            currentLevel = new Level(1); // TODO: handle difficulty
            currentLevel.Start(e, playerPrefab, enemyPrefab);
            gameObject.SetActive(true);
        }

        public void EndCombat()
        {
            currentLevel.End();
            gameObject.SetActive(false);
        }

        private void ResetAllCombatStageSetting()
        {
            cameraRoot.position = new Vector3(0, 0, -10);
            background01.position = new Vector3(0, 0, 0);
            background02.position = new Vector3(7.34f, 0, 0);
            currentMapIndex = 0;
        }

        private void Update()
        {
            if (cameraRoot.position.x >= 7.34f * (currentMapIndex + 1))
            {
                switch (currentMapIndex % 2)
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

            CombatActor playerActor = CombatActorContainer.GetAnyUnitByCamp(CombatActor.ActorInfo.Camp.Player);
            if (playerActor == null)
            {
                return;
            }

            if (isShaking)
            {
                return;
            }

            cameraRoot.transform.position = Vector3.MoveTowards(cameraRoot.transform.position, playerActor.transform.position + cameraOffset, 10f * Time.deltaTime);
        }
    }
}