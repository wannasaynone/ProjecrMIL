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

        public static void ClearAll()
        {
            for (int i = 0; i < units.Count; i++)
            {
                UnityEngine.Object.Destroy(units[i].Actor.gameObject);
            }
            units.Clear();
        }

        public static CombatUnit GetAnyUnitByCamp(CombatUnit.Camp camp)
        {
            foreach (var unit in units)
            {
                if (unit.camp == camp)
                    return unit;
            }

            return null;
        }

        public static CombatUnit GetCloestUnitByCamp(CombatUnit.Camp camp, UnityEngine.Vector3 position)
        {
            CombatUnit cloestUnit = null;
            float minDistance = float.MaxValue;

            foreach (var unit in units)
            {
                if (unit.camp == camp)
                {
                    float distance = UnityEngine.Vector3.Distance(unit.Actor.transform.position, position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        cloestUnit = unit;
                    }
                }
            }

            return cloestUnit;
        }

        public static List<CombatUnit> GetAllUnitInRange(CombatUnit.Camp camp, UnityEngine.Vector3 position, float range)
        {
            List<CombatUnit> unitsInRange = new List<CombatUnit>();

            foreach (var unit in units)
            {
                if (unit.camp == camp)
                {
                    float distance = UnityEngine.Vector3.Distance(unit.Actor.transform.position, position);
                    if (distance < range)
                    {
                        unitsInRange.Add(unit);
                    }
                }
            }

            return unitsInRange;
        }
    }
}