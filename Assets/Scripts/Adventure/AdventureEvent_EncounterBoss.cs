using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMIL.Adventure
{
    public class AdventureEvent_EncounterBoss : AdventureEventBase
    {
        private int difficulty;

        public AdventureEvent_EncounterBoss(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public override void Execute()
        {
            GameEvent.EventBus.Publish(new GameEvent.OnAdventureEventCreated_EncounterBoss()
            {
                difficulty = difficulty
            });
        }
    }
}