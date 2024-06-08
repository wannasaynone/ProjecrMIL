using KahaGameCore.GameData.Implemented;
using ProjectMIL.Data;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Combat.CombatManager combatManager;
        [SerializeField] private Data.GameConfig gameConfig;
        private Adventure.AdventureManager adventureManager;
        private Utlity.ContextHandler contextHandler;
        private GameStaticDataManager gameStaticDataManager;
        private GameStaticDataDeserializer gameStaticDataDeserializer;
        private Player player;

        private void Awake()
        {
            gameStaticDataManager = new GameStaticDataManager();
            gameStaticDataDeserializer = new GameStaticDataDeserializer();
            gameStaticDataManager.Add<Data.ExpData>(gameStaticDataDeserializer.Read<Data.ExpData[]>(Resources.Load<TextAsset>("Data/ExpData").text));
            gameStaticDataManager.Add<Data.ContextData>(gameStaticDataDeserializer.Read<Data.ContextData[]>(Resources.Load<TextAsset>("Data/ContextData").text));

            contextHandler = new Utlity.ContextHandler(gameStaticDataManager);

            adventureManager = new Adventure.AdventureManager();
            adventureManager.Initialize(contextHandler);

            uiManager.Initialize();

            Element.UserInterfaceContextSetter[] userInterfaceContextSetters = FindObjectsOfType<Element.UserInterfaceContextSetter>(true);
            foreach (Element.UserInterfaceContextSetter userInterfaceContextSetter in userInterfaceContextSetters)
            {
                userInterfaceContextSetter.SetUp(contextHandler);
            }

            player = new Player(gameStaticDataManager.GetAllGameData<Data.ExpData>(), gameConfig);
            player.Initail();

            EventBus.Subscribe<OnCombatStartCalled>(OnCombatStartCalled);
        }

        private void OnCombatStartCalled(OnCombatStartCalled e)
        {
            uiManager.gameObject.SetActive(false);
            combatManager.StartCombat(e);
        }

        private bool test_isInCombat = false;
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!test_isInCombat)
                {
                    test_isInCombat = true;
                    SaveData clone = player.GetSaveDataClone();
                    EventBus.Publish(new OnCombatStartCalled
                    {
                        difficulty = 0,
                        maxHP = clone.maxHP,
                        attack = clone.attack,
                        defense = clone.defense,
                        speed = clone.speed,
                        critical = clone.critical,
                        criticalResistance = clone.criticalResistance,
                        effectiveness = clone.effectiveness,
                        effectivenessResistance = clone.effectivenessResistance
                    });
                }
                else
                {
                    test_isInCombat = false;
                    combatManager.EndCombat();
                    uiManager.gameObject.SetActive(true);
                }
            }
        }
    }
}