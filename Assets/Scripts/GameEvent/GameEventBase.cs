namespace ProjectMIL.GameEvent
{
    public class GameEventBase { }
    public class OnBottomBarButtonPressed : GameEventBase
    {
        public int buttonIndex;
    }
    public class OnForceButtomBarButtonEnable : GameEventBase
    {
        public int buttonIndex;
    }
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
    public class OnPlayerValueUpdated : GameEventBase
    {
        public int level;
        public int requireExp;
        public int exp;
        public int gold;
        public int maxHP;
        public int defense;
        public int attack;
        public int speed;
        public int critical;
        public int criticalResistance;
        public int effectiveness;
        public int effectivenessResistance;
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
    public class OnTryLevelUpCalled : GameEventBase
    {
        public int tryAddLevel;
    }
    public class OnLevelUpdated : GameEventBase
    {
        public int oldLevel;
        public int currentLevel;
        public int currentExp;
        public int requireExp;
    }
    public class OnAttackButtonPressed : GameEventBase
    {
        public string attackName;
    }
    public class OnCombatStartCalled : GameEventBase { }
}