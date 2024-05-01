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
            adventureManager = new Adventure.AdventureManager();
            adventureManager.Initail();

            uiManager.Initail();

            gameStaticDataManager = new GameStaticDataManager();
            gameStaticDataDeserializer = new GameStaticDataDeserializer();

            gameStaticDataManager.Add<Data.ExpData>(gameStaticDataDeserializer.Read<Data.ExpData[]>(Resources.Load<TextAsset>("Data/ExpData").text));

            player = new Player(gameStaticDataManager.GetAllGameData<Data.ExpData>());
            player.Initail();
        }
    }
}