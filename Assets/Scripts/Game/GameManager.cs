using KahaGameCore.GameData.Implemented;
using UnityEngine;

namespace ProjectMIL.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        private Adventure.AdventureManager adventureManager;
        private GameStaticDataManager gameStaticDataManager;
        private GameStaticDataDeserializer gameStaticDataDeserializer;
        private Player player;

        private void Awake()
        {
            gameStaticDataManager = new GameStaticDataManager();
            gameStaticDataDeserializer = new GameStaticDataDeserializer();
            gameStaticDataManager.Add<Data.ExpData>(gameStaticDataDeserializer.Read<Data.ExpData[]>(Resources.Load<TextAsset>("Data/ExpData").text));
            gameStaticDataManager.Add<Data.ContextData>(gameStaticDataDeserializer.Read<Data.ContextData[]>(Resources.Load<TextAsset>("Data/ContextData").text));

            adventureManager = new Adventure.AdventureManager();
            adventureManager.Initial(gameStaticDataManager);

            uiManager.Initail();

            player = new Player(gameStaticDataManager.GetAllGameData<Data.ExpData>());
            player.Initail();
        }
    }
}