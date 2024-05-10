namespace ProjectMIL.GameEvent
{
    public class GameEventBase { }
    public class OnAdventureButtonPressed : GameEventBase { }
    public class OnAdventureEventCreated_Exp : GameEventBase
    {
        public string title;
        public string description;
        public int addExp;
    }
    public class OnAdventureEventCreated_Gold : GameEventBase
    {
        public string title;
        public string description;
        public int addGold;
    }
    public class OnPlayerInitialed : GameEventBase
    {
        public int level;
        public int requireExp;
        public int exp;
        public int gold;
    }
    public class OnExpValueUpdated : GameEventBase
    {
        public int oldValue;
        public int addValue;
        public int newValue;
        public int level;
        public int requireExp;
    }
    public class OnGoldValueUpdated : GameEventBase
    {
        public int oldValue;
        public int addValue;
        public int newValue;
    }
    public class OnAdventureProgressBarAnimationEnded : GameEventBase { }
    public class OnAdventureEventResultPanelDisabled : GameEventBase { }
}