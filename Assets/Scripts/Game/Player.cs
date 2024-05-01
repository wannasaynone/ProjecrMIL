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
        private ExpData[] expDatas;

        public void Initail()
        {
            saveData = new SaveData
            {
                level = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_LEVEL", 1), // for now, we will use PlayerPrefs to save the level value
                exp = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_EXP", 0) // for now, we will use PlayerPrefs to save the exp value
            };

            EventBus.Subscribe<OnAdventureEventCreated>(OnAdventureEventCreated);

            EventBus.Publish(new OnPlayerInitialed
            {
                level = saveData.level,
                exp = saveData.exp,
                requireExp = GetRequireWithCurrentLevel()
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

        private void OnAdventureEventCreated(OnAdventureEventCreated created)
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
    }
}