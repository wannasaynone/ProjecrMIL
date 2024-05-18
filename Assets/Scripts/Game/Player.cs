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
                gold = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_GOLD", 0), // for now, we will use PlayerPrefs to save the gold value
                attack = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_ATTACK", 100), // for now, we will use PlayerPrefs to save the attack value
                defense = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_DEFENSE", 100), // for now, we will use PlayerPrefs to save the defense value
                maxHP = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_MAXHP", 100), // for now, we will use PlayerPrefs to save the maxHP value
                speed = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_SPEED", 100), // for now, we will use PlayerPrefs to save the speed value
                critical = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_CRITICAL", 0), // for now, we will use PlayerPrefs to save the critical value
                criticalResistance = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_CRITICALRESISTANCE", 0), // for now, we will use PlayerPrefs to save the criticalResistance value
                effectiveness = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_EFFECTIVENESS", 0), // for now, we will use PlayerPrefs to save the effectiveness value
                effectivenessResistance = UnityEngine.PlayerPrefs.GetInt("PLYAYER_SAVE_EFFECTIVENESSRESISTANCE", 0), // for now, we will use PlayerPrefs to save the effectivenessResistance value
            };

            EventBus.Subscribe<OnAdventureEventCreated_Exp>(OnAdventureEventCreated_Exp);
            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated_Gold);
            EventBus.Subscribe<OnTryLevelUpCalled>(OnTryLevelUpCalled);

            EventBus.Publish(new OnPlayerInitialed
            {
                level = saveData.level,
                exp = saveData.exp,
                requireExp = GetRequireWithCurrentLevel(),
                gold = saveData.gold,
                attack = saveData.attack,
                defense = saveData.defense,
                maxHP = saveData.maxHP,
                speed = saveData.speed,
                critical = saveData.critical,
                criticalResistance = saveData.criticalResistance,
                effectiveness = saveData.effectiveness,
                effectivenessResistance = saveData.effectivenessResistance
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

            EventBus.Publish(new OnExpValueUpdated
            {
                oldValue = oldValue,
                addValue = created.addExp,
                newValue = saveData.exp,
                level = saveData.level,
                requireExp = GetRequireWithCurrentLevel()
            });
        }

        private void OnTryLevelUpCalled(OnTryLevelUpCalled e)
        {
            int oldLevel = saveData.level;

            for (int i = 0; i < e.tryAddLevel; i++)
            {
                if (saveData.exp >= GetRequireWithCurrentLevel())
                {
                    saveData.exp -= GetRequireWithCurrentLevel();
                    saveData.level++;
                }
                else
                {
                    break;
                }
            }

            if (oldLevel != saveData.level)
            {
                EventBus.Publish(new OnLevelUpdated
                {
                    oldLevel = oldLevel,
                    currentLevel = saveData.level,
                    currentExp = saveData.exp,
                    requireExp = GetRequireWithCurrentLevel()
                });
            }
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