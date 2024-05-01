namespace ProjectMIL.GameEvent
{
    public class GameEventBase { }
    public class OnAdventureButtonPressed : GameEventBase { }
    public class OnAdventureEventCreated : GameEventBase
    {
        public int addExp;
    }
    public class OnAdventureProgressBarAnimationEnded : GameEventBase { }
}