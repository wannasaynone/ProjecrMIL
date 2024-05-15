using KahaGameCore.GameData.Implemented;
using UnityEngine;

namespace ProjectMIL.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
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
            adventureManager.Initial(contextHandler);

            uiManager.Initail();

            Element.UserInterfaceContextSetter[] userInterfaceContextSetters = FindObjectsOfType<Element.UserInterfaceContextSetter>(true);
            foreach (Element.UserInterfaceContextSetter userInterfaceContextSetter in userInterfaceContextSetters)
            {
                userInterfaceContextSetter.SetUp(contextHandler);
            }

            player = new Player(gameStaticDataManager.GetAllGameData<Data.ExpData>());
            player.Initail();
        }
    }
}