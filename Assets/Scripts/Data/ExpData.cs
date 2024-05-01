using KahaGameCore.GameData;

namespace ProjectMIL.Data
{
    public class ExpData : IGameData
    {
        public int ID { get; private set; }
        public int RequireExp { get; private set; }
        public int RqeuireMoney { get; private set; }
    }
}