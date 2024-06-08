using System;
using System.Collections.Generic;

namespace ProjectMIL.GameEvent
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Action<GameEventBase>>> eventHandlers = new Dictionary<Type, List<Action<GameEventBase>>>();
        private static readonly Dictionary<int, Action<GameEventBase>> originHashCodeToWrapperHandler = new Dictionary<int, Action<GameEventBase>>();

        public static void ForceClearAll()
        {
            eventHandlers.Clear();
        }

        public static void Subscribe<T>(Action<T> handler) where T : GameEventBase
        {
            var eventType = typeof(T);

            if (!eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType] = new List<Action<GameEventBase>>();
            }

            Action<GameEventBase> wrapperHandler = (eventBase) => handler((T)eventBase);
            originHashCodeToWrapperHandler.Add(handler.GetHashCode(), wrapperHandler);

            eventHandlers[eventType].Add(wrapperHandler);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : GameEventBase
        {
            var eventType = typeof(T);

            if (!eventHandlers.ContainsKey(eventType))
            {
                return;
            }

            for (int i = 0; i < eventHandlers[eventType].Count; i++)
            {
                if (originHashCodeToWrapperHandler[handler.GetHashCode()] == eventHandlers[eventType][i])
                {
                    eventHandlers[eventType].RemoveAt(i);
                    originHashCodeToWrapperHandler.Remove(handler.GetHashCode());
                    break;
                }
            }
        }

        public static void Publish<T>(T eventToPublish) where T : GameEventBase
        {
            var eventType = typeof(T);

            if (!eventHandlers.ContainsKey(eventType))
            {
                return;
            }

            for (int i = 0; i < eventHandlers[eventType].Count; i++)
            {
                if (eventHandlers[eventType][i] == null)
                {
                    eventHandlers[eventType].RemoveAt(i);
                    i--;
                    continue;
                }

                eventHandlers[eventType][i]?.Invoke(eventToPublish);
            }
        }
    }

}