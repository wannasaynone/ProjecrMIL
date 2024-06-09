using System;
using System.Collections.Generic;

namespace ProjectMIL.Combat
{
    public static class CombatActorContainer
    {
        public static List<CombatActor> actors = new List<CombatActor>();

        public static void AddActor(CombatActor actor)
        {
            if (actors.Contains(actor))
                throw new Exception("Unit already exists in container");

            actors.Add(actor);
        }

        public static void RemoveActor(CombatActor actor)
        {
            if (!actors.Contains(actor))
                throw new Exception("Unit does not exist in container");

            actors.Remove(actor);
        }

        public static CombatActor GetAnyActorByCamp(CombatActor.ActorInfo.Camp camp, bool ingnoreDead)
        {
            foreach (var actor in actors)
            {
                if (actor.Info.ActorCamp == camp && (ingnoreDead || (!ingnoreDead && actor.Info.currentHP > 0)))
                    return actor;
            }

            return null;
        }

        public static CombatActor GetCloestActorByCamp(CombatActor.ActorInfo.Camp camp, UnityEngine.Vector3 position)
        {
            CombatActor cloestActor = null;
            float minDistance = float.MaxValue;

            foreach (var actor in actors)
            {
                if (actor.Info.ActorCamp == camp && actor.Info.currentHP > 0)
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

        public static List<CombatActor> GetAllActorInRange(CombatActor.ActorInfo.Camp camp, UnityEngine.Vector3 position, float range)
        {
            List<CombatActor> actorsInRange = new List<CombatActor>();

            foreach (var actor in actors)
            {
                if (actor.Info.ActorCamp == camp && actor.Info.currentHP > 0)
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

        public static CombatActor GetActorByInstanceID(int attackerActorInstanceID)
        {
            for (int i = 0; i < actors.Count; i++)
            {
                if (actors[i].GetInstanceID() == attackerActorInstanceID)
                    return actors[i];
            }

            return null;
        }
    }
}