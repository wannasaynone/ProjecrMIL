using ProjectMIL.Data;
using ProjectMIL.GameEvent;

namespace ProjectMIL.Game
{
    public class Player
    {
        public Player(ExpData[] expDatas)
        {
            this.expDatas = expDatas;
        }

        private SaveData saveData;
        private readonly ExpData[] expDatas;

        public void Initail()
        {
            saveData = new SaveData
            {
                level = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_LEVEL", 1), // for now, we will use PlayerPrefs to save the level value
                exp = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_EXP", 0), // for now, we will use PlayerPrefs to save the exp value
                gold = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_GOLD", 0) // for now, we will use PlayerPrefs to save the gold value
            };

            EventBus.Subscribe<OnAdventureEventCreated_Exp>(OnAdventureEventCreated_Exp);
            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated_Gold);

            EventBus.Publish(new OnPlayerInitialed
            {
                level = saveData.level,
                exp = saveData.exp,
                requireExp = GetRequireWithCurrentLevel(),
                gold = saveData.gold
            });
        }

        private int GetRequireWithCurrentLevel()
        {
            for (int i = 0; i < expDatas.Length; i++)
            {
                if (expDatas[i].ID == saveData.level)
                {
                    return expDatas[i].RequireExp;
                }
            }

            return 0;
        }

        private void OnAdventureEventCreated_Exp(OnAdventureEventCreated_Exp created)
        {
            int oldValue = saveData.exp;
            saveData.exp += created.addExp;

            while (saveData.exp >= GetRequireWithCurrentLevel())
            {
                saveData.exp -= GetRequireWithCurrentLevel();
                saveData.level++;
            }

            EventBus.Publish(new OnExpValueUpdated
            {
                oldValue = oldValue,
                addValue = created.addExp,
                newValue = saveData.exp,
                level = saveData.level,
                requireExp = GetRequireWithCurrentLevel()
            });
        }

        private void OnAdventureEventCreated_Gold(OnAdventureEventCreated_Gold created)
        {
            saveData.gold += created.addGold;
            EventBus.Publish(new OnGoldValueUpdated
            {
                oldValue = saveData.gold - created.addGold,
                addValue = created.addGold,
                newValue = saveData.gold
            });
        }
    }
}