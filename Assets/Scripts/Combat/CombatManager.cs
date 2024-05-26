
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
            CombatActor playerActor = CombatActorContainer.GetAnyUnitByCamp(CombatActor.ActorInfo.Camp.Player);
            if (playerActor == null)
            {
                return;
            }

            cameraRoot.transform.position = Vector3.MoveTowards(cameraRoot.transform.position, playerActor.transform.position + cameraOffset, 10f * Time.deltaTime);
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
        }
    }
}