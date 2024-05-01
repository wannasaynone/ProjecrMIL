namespace ProjectMIL.GameEvent
{
    public class GameEventBase { }
    public class OnAdventureButtonPressed : GameEventBase { }
    public class OnAdventureEventCreated : GameEventBase
    {
        public int titleContextID;
        public int descContextID;
        public int addExp;
    }
    public class OnPlayerInitialed : GameEventBase
    {
        public int level;
        public int requireExp;
        public int exp;
    }
    public class OnExpValueUpdated : GameEventBase
    {
        public int oldValue;
        public int addValue;
        public int newValue;
        public int level;
        public int requireExp;
    }
    public class OnAdventureProgressBarAnimationEnded : GameEventBase { }
    public class OnAdventureEventResultPanelDisabled : GameEventBase { }
}