using UnityEngine;

namespace ProjectMIL.Adventure
{
    public class AdventureManager
    {
        public AdventureManager()
        {
            GameEvent.EventBus.Subscribe<GameEvent.OnAdventureButtonPressed>(OnAdventureButtonPressed);
        }

        private void OnAdventureButtonPressed(GameEvent.OnAdventureButtonPressed eventToPublish)
        {
            Debug.Log("Adventure button pressed");
        }
    }
}