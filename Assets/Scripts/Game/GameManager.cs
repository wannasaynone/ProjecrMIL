using UnityEngine;

namespace ProjectMIL.Game
{
    public class GameManager : MonoBehaviour
    {
        private Adventure.AdventureManager adventureManager;

        private void Awake()
        {
            adventureManager = new Adventure.AdventureManager();
        }
    }
}