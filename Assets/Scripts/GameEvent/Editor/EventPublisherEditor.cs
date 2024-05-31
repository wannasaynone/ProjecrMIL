using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.AI;
using JsonFx.Json;

namespace ProjectMIL.GameEvent.Editor
{
    public class EventPublisherEditor : EditorWindow
    {
        [MenuItem("Tools/Game Event Publisher")]
        public static void ShowWindow()
        {
            GetWindow<EventPublisherEditor>("Game Event Publisher");
        }

        private void OnGUI()
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
                    typeof(EventBus)
                        .GetMethod("Publish")
                        .MakeGenericMethod(type)
                        .Invoke(null, new object[] { instance });
                }
            }

            HorizontalLine();

            if (GUILayout.Button("Publish On Adventure Event get 500 exp"))
            {
                EventBus.Publish(new OnAdventureEventCreated_Exp()
                {
                    addExp = 500,
                    title = "測試 500 經驗",
                    description = "獲得了 {0} 點經驗值"
                });
            }

            if (GUILayout.Button("Publish On Adventure Event get 1000 exp"))
            {
                EventBus.Publish(new OnAdventureEventCreated_Exp()
                {
                    addExp = 1000,
                    title = "測試 1000 經驗",
                    description = "獲得了 {0} 點經驗值"
                });
            }

            if (GUILayout.Button("Publish Try Level Up"))
            {
                EventBus.Publish(new OnTryLevelUpCalled()
                {
                    tryAddLevel = 1
                });
            }

            if (GUILayout.Button("Publish Test Level 13 -> 14"))
            {
                EventBus.Publish(new OnLevelUpdated()
                {
                    oldLevel = 13,
                    currentLevel = 14,
                    currentExp = 0,
                    requireExp = 0
                });
            }

            if (GUILayout.Button("Test Json"))
            {
                string json = JsonWriter.Serialize(new OnLevelUpdated()
                {
                    oldLevel = 13,
                    currentLevel = 14,
                    currentExp = 0,
                    requireExp = 0
                });

                Debug.Log(json);

                string doubleJson = JsonWriter.Serialize(json);

                Debug.Log(doubleJson);
            }
        }

        private static void HorizontalLine()
        {
            GUIStyle horizontalLine;
            horizontalLine = new GUIStyle();
            horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            horizontalLine.margin = new RectOffset(0, 0, 4, 4);
            horizontalLine.fixedHeight = 1;

            GUILayout.Space(10);

            var c = GUI.color;
            GUI.color = Color.gray;
            GUILayout.Box(GUIContent.none, horizontalLine);
            GUI.color = c;

            GUILayout.Space(10);
        }
    }
}

