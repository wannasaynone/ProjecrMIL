namespace ProjectMIL.Adventure
{
    public class AdventureEvent_EncounterEnemy : AdventureEventBase
    {
        private int difficulty = 0;

        public AdventureEvent_EncounterEnemy(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public override void Execute()
        {
            GameEvent.EventBus.Publish(new GameEvent.OnAdventureEventCreated_EncounterEnemy()
            {
                difficulty = difficulty
            });
        }
    }
}
