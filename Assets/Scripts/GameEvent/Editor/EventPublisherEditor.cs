using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace ProjectMIL.GameEvent.Editor
{
    public class EventPublisherEditor : EditorWindow
    {
        [MenuItem("Tools/Game Event Publisher")]
        public static void ShowWindow()
        {
            GetWindow<EventPublisherEditor>("Game Event Publisher");
        }

        void OnGUI()
        {
            GUILayout.Label("Publish Game Events", EditorStyles.boldLabel);

            // 獲取ProjectMIL.GameEvent命名空間中所有繼承自GameEventBase的類
            var gameEventTypes = Assembly.GetAssembly(typeof(GameEventBase))
                                         .GetTypes()
                                         .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(GameEventBase)) && t.Namespace == "ProjectMIL.GameEvent");

            foreach (Type type in gameEventTypes)
            {
                if (GUILayout.Button("Publish " + type.Name))
                {
                    // 使用反射來創建對應類的實例並發送事件
                    var instance = Activator.CreateInstance(type);
                    typeof(GameEvent.EventBus)
                        .GetMethod("Publish")
                        .MakeGenericMethod(type)
                        .Invoke(null, new object[] { instance });
                }
            }
        }
    }
}

