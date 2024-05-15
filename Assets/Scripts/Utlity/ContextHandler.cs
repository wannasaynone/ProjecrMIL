using KahaGameCore.GameData.Implemented;

namespace ProjectMIL.Utlity
{
    public class ContextHandler
    {
        private readonly GameStaticDataManager gameStaticDataManager;

        public ContextHandler(GameStaticDataManager gameStaticDataManager)
        {
            this.gameStaticDataManager = gameStaticDataManager;
        }

        public string GetContext(int contextID)
        {
            Data.ContextData contextData = gameStaticDataManager.GetGameData<Data.ContextData>(contextID);

            if (contextData == null)
            {
                return "NULL";
            }

            return gameStaticDataManager.GetGameData<Data.ContextData>(contextID).zh_tw;
        }
    }
}