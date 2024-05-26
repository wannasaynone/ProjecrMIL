
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
            ResetAll();
            gameObject.SetActive(true);
        }

        private void ResetAll()
        {
            cameraRoot.position = new Vector3(0, 0, -10);
            background01.position = new Vector3(0, 0, 0);
            background02.position = new Vector3(7.34f, 0, 0);
            currentMapIndex = 0;

            CombatUnit playerUnit = new CombatUnit(Instantiate(playerPrefab), CombatUnit.Camp.Player);
            playerUnit.Actor.transform.position = new Vector3(-2f, -1f, 0);
            playerUnit.Actor.Initialize();
            CombatUnitContainer.AddUnit(playerUnit);

            CombatUnit enemyUnit = new CombatUnit(Instantiate(enemyPrefab), CombatUnit.Camp.Enemy);
            enemyUnit.Actor.transform.position = new Vector3(2f, -1f, 0);
            enemyUnit.Actor.Initialize();
            CombatUnitContainer.AddUnit(enemyUnit);
        }

        private void Update()
        {
            CombatUnit playerUnit = CombatUnitContainer.GetAnyUnitByCamp(CombatUnit.Camp.Player);
            if (playerUnit == null)
            {
                return;
            }

            cameraRoot.transform.position = Vector3.MoveTowards(cameraRoot.transform.position, playerUnit.Actor.transform.position + cameraOffset, 10f * Time.deltaTime);
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