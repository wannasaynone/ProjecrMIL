namespace ProjectMIL.Combat
{
    public class CombatUnit
    {
        public enum Camp
        {
            Player,
            Enemy
        }

        public CombatActor Actor { get; private set; }
        public Camp camp;

        public CombatUnit(CombatActor actor, Camp camp)
        {
            Actor = actor;
            this.camp = camp;
        }
    }
}