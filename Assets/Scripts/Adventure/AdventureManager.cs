using System.Collections.Generic;
using UnityEngine;

namespace ProjectMIL.Adventure
{
    public class AdventureManager
    {
        private List<AdventureEventBase> events = new List<AdventureEventBase>();

        public void Initialize()
        {
            // TODO: handle event save/load
            for (int i = 0; i < 26; i++)
            {
                events.Add(new AdventureEvent_ExpAndGold());
            }

            SuffuleEvents();

            for (int i = 0; i < 4; i++)
            {
                events.Insert(Random.Range(events.Count / 4 * i, events.Count / 4 * (i + 1)), new AdventureEvent_EncounterEnemy(i * 2));
            }

            GameEvent.EventBus.Subscribe<GameEvent.OnAdventureButtonPressed>(OnAdventureButtonPressed);
        }

        private void SuffuleEvents()
        {
            for (int i = 0; i < events.Count; i++)
            {
                AdventureEventBase temp = events[i];
                int randomIndex = Random.Range(i, events.Count);
                events[i] = events[randomIndex];
                events[randomIndex] = temp;
            }
        }

        private void OnAdventureButtonPressed(GameEvent.OnAdventureButtonPressed eventToPublish)
        {
            if (events.Count == 0)
            {
                Debug.Log("No more events");
                return;
            }

            AdventureEventBase adventureEvent = events[0];
            events.RemoveAt(0);
            adventureEvent.Execute();

            if (events.Count == 0)
            {
                events.Add(new AdventureEvent_EncounterBoss(11));
            }
        }
    }
}