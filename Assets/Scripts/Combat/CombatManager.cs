
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform playerCharacter;
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private Transform background01;
        [SerializeField] private Transform background02;

        private int currentIndex = 0;

        public void StartCombat()
        {
            ResetAll();
            gameObject.SetActive(true);
        }

        private void ResetAll()
        {
            playerCharacter.position = new Vector3(-2f, -1f, 0);
            cameraRoot.position = new Vector3(0, 0, -10);
            background01.position = new Vector3(0, 0, 0);
            background02.position = new Vector3(7.34f, 0, 0);
        }

        private void Update()
        {
            cameraRoot.transform.position = Vector3.MoveTowards(cameraRoot.transform.position, playerCharacter.position + cameraOffset, Time.deltaTime);
            if (cameraRoot.position.x >= 7.34f * (currentIndex + 1))
            {
                switch (currentIndex % 2)
                {
                    case 0:
                        background01.position = new Vector3(7.34f * (currentIndex + 2), 0, 0);
                        break;
                    case 1:
                        background02.position = new Vector3(7.34f * (currentIndex + 2), 0, 0);
                        break;
                }
                currentIndex++;
            }
        }
    }
}