
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

        private void Awake()
        {
            EventBus.Subscribe<OnStartToHit>(OnStartToHit);
        }

        private void OnStartToHit(OnStartToHit e)
        {
            if (isShaking)
                return;

            isShaking = true;
            CombatActor playerActor = CombatActorContainer.GetAnyUnitByCamp(CombatActor.ActorInfo.Camp.Player);
            cameraRoot.DOMove(playerActor.transform.position + cameraOffset, 0.1f).OnComplete(() =>
            {
                cameraRoot.DOShakePosition(0.5f, 0.25f, 10, 90f, false, true).OnComplete(() => isShaking = false);
            });
        }

        public void StartCombat()
        {
            CombatActorContainer.ClearAll();
            ResetAllCombatStageSetting();
            gameObject.SetActive(true);
        }

        private void ResetAllCombatStageSetting()
        {
            cameraRoot.position = new Vector3(0, 0, -10);
            background01.position = new Vector3(0, 0, 0);
            background02.position = new Vector3(7.34f, 0, 0);
            currentMapIndex = 0;

            CombatActor playerActor = Instantiate(playerPrefab);
            playerActor.transform.position = new Vector3(-2f, -1f, 0);
            playerActor.Initialize(new CombatActor.ActorInfo(CombatActor.ActorInfo.Camp.Player));
            CombatActorContainer.AddActor(playerActor);

            CombatActor enemyActor = Instantiate(enemyPrefab);
            enemyActor.transform.position = new Vector3(2f, -1f, 0);
            enemyActor.Initialize(new CombatActor.ActorInfo(CombatActor.ActorInfo.Camp.Enemy));
            CombatActorContainer.AddActor(enemyActor);
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