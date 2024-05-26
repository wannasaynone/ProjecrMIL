using System.Collections.Generic;

namespace ProjectMIL.Combat
{
    public static class CombatActorContainer
    {
        public static List<CombatActor> actors = new List<CombatActor>();

        public static void AddActor(CombatActor actor)
        {
            if (actors.Contains(actor))
                throw new System.Exception("Unit already exists in container");

            actors.Add(actor);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                UnityEngine.Object.Destroy(actors[i].gameObject);
            }
            actors.Clear();
        }

        public static CombatActor GetAnyUnitByCamp(CombatActor.ActorInfo.Camp camp)
        {
            foreach (var actor in actors)
            {
                if (actor.Info.camp == camp)
                    return actor;
            }

            return null;
        }

        public static CombatActor GetCloestUnitByCamp(CombatActor.ActorInfo.Camp camp, UnityEngine.Vector3 position)
        {
            CombatActor cloestActor = null;
            float minDistance = float.MaxValue;

            foreach (var actor in actors)
            {
                if (actor.Info.camp == camp)
                {
                    float distance = UnityEngine.Vector3.Distance(actor.transform.position, position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        cloestActor = actor;
                    }
                }
            }

            return cloestActor;
        }

        public static List<CombatActor> GetAllUnitInRange(CombatActor.ActorInfo.Camp camp, UnityEngine.Vector3 position, float range)
        {
            List<CombatActor> actorsInRange = new List<CombatActor>();

            foreach (var actor in actors)
            {
                if (actor.Info.camp == camp)
                {
                    float distance = UnityEngine.Vector3.Distance(actor.transform.position, position);
                    if (distance < range)
                    {
                        actorsInRange.Add(actor);
                    }
                }
            }

            return actorsInRange;
        }
    }
}