using System.Collections.Generic;

namespace ProjectMIL.Combat
{
    public static class CombatUnitContainer
    {
        public static List<CombatUnit> units = new List<CombatUnit>();

        public static void AddUnit(CombatUnit unit)
        {
            if (units.Contains(unit))
                throw new System.Exception("Unit already exists in container");

            units.Add(unit);
        }

        public static void RemoveUnit(CombatUnit unit)
        {
            units.Remove(unit);
        }
    }
}