using ProjectMIL.Data;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Game
{
    public class Player
    {
        public Player(ExpData[] expDatas, GameConfig gameConfig)
        {
            this.expDatas = expDatas;
            this.gameConfig = gameConfig;
        }

        private SaveData saveData;

        public SaveData GetSaveDataClone()
        {
            return new SaveData
            {
                level = saveData.level,
                exp = saveData.exp,
                gold = saveData.gold,
                attack = saveData.attack,
                defense = saveData.defense,
                maxHP = saveData.maxHP,
                speed = saveData.speed,
                critical = saveData.critical,
                criticalResistance = saveData.criticalResistance,
                effectiveness = saveData.effectiveness,
                effectivenessResistance = saveData.effectivenessResistance
            };
        }

        private readonly ExpData[] expDatas;
        private readonly GameConfig gameConfig;

        public void Initail()
        {
            saveData = new SaveData
            {
                level = PlayerPrefs.GetInt("PLYAYER_SAVE_LEVEL", 1), // for now, we will use PlayerPrefs to save the level value
                exp = PlayerPrefs.GetInt("PLYAYER_SAVE_EXP", 0), // for now, we will use PlayerPrefs to save the exp value
                gold = PlayerPrefs.GetInt("PLYAYER_SAVE_GOLD", 0), // for now, we will use PlayerPrefs to save the gold value
                maxHP = PlayerPrefs.GetInt("PLYAYER_SAVE_MAXHP", 100), // for now, we will use PlayerPrefs to save the maxHP value
                attack = PlayerPrefs.GetInt("PLYAYER_SAVE_ATTACK", 100), // for now, we will use PlayerPrefs to save the attack value
                defense = PlayerPrefs.GetInt("PLYAYER_SAVE_DEFENSE", 100), // for now, we will use PlayerPrefs to save the defense value
                speed = PlayerPrefs.GetInt("PLYAYER_SAVE_SPEED", 100), // for now, we will use PlayerPrefs to save the speed value
                critical = PlayerPrefs.GetInt("PLYAYER_SAVE_CRITICAL", 0), // for now, we will use PlayerPrefs to save the critical value
                criticalResistance = PlayerPrefs.GetInt("PLYAYER_SAVE_CRITICALRESISTANCE", 0), // for now, we will use PlayerPrefs to save the criticalResistance value
                effectiveness = PlayerPrefs.GetInt("PLYAYER_SAVE_EFFECTIVENESS", 0), // for now, we will use PlayerPrefs to save the effectiveness value
                effectivenessResistance = PlayerPrefs.GetInt("PLYAYER_SAVE_EFFECTIVENESSRESISTANCE", 0), // for now, we will use PlayerPrefs to save the effectivenessResistance value
            };

            EventBus.Subscribe<OnAdventureEventCreated_Exp>(OnAdventureEventCreated_Exp);
            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated_Gold);
            EventBus.Subscribe<OnTryLevelUpCalled>(OnTryLevelUpCalled);

            EventBus.Publish(new OnPlayerValueUpdated
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
            SaveCurrent();

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
            SaveCurrent();
            EventBus.Publish(new OnGoldValueUpdated
            {
                oldValue = saveData.gold - created.addGold,
                addValue = created.addGold,
                newValue = saveData.gold
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
                    ForceLevelUp(1);
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

        private void ForceLevelUp(int addLevel)
        {
            saveData.level += addLevel;
            int addPoints = gameConfig.AddStatusValuePerLevel * addLevel;

            // 建立一個欄位數組
            int[] fields =
            {
                saveData.maxHP,
                saveData.defense,
                saveData.attack,
                saveData.speed,
                saveData.critical,
                saveData.criticalResistance,
                saveData.effectiveness,
                saveData.effectivenessResistance
            };

            // 隨機分配
            while (addPoints > 0)
            {
                int randomIndex = Random.Range(0, fields.Length);
                AddStatus(ref addPoints, ref fields[randomIndex]);
            }

            // 更新 saveData 的欄位
            saveData.maxHP = fields[0];
            saveData.defense = fields[1];
            saveData.attack = fields[2];
            saveData.speed = fields[3];
            saveData.critical = fields[4];
            saveData.criticalResistance = fields[5];
            saveData.effectiveness = fields[6];
            saveData.effectivenessResistance = fields[7];

            SaveCurrent();

            EventBus.Publish(new OnPlayerValueUpdated
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

        private void SaveCurrent()
        {
            PlayerPrefs.SetInt("PLYAYER_SAVE_LEVEL", saveData.level);
            PlayerPrefs.SetInt("PLYAYER_SAVE_EXP", saveData.exp);
            PlayerPrefs.SetInt("PLYAYER_SAVE_GOLD", saveData.gold);
            PlayerPrefs.SetInt("PLYAYER_SAVE_MAXHP", saveData.maxHP);
            PlayerPrefs.SetInt("PLYAYER_SAVE_ATTACK", saveData.attack);
            PlayerPrefs.SetInt("PLYAYER_SAVE_DEFENSE", saveData.defense);
            PlayerPrefs.SetInt("PLYAYER_SAVE_SPEED", saveData.speed);
            PlayerPrefs.SetInt("PLYAYER_SAVE_CRITICAL", saveData.critical);
            PlayerPrefs.SetInt("PLYAYER_SAVE_CRITICALRESISTANCE", saveData.criticalResistance);
            PlayerPrefs.SetInt("PLYAYER_SAVE_EFFECTIVENESS", saveData.effectiveness);
            PlayerPrefs.SetInt("PLYAYER_SAVE_EFFECTIVENESSRESISTANCE", saveData.effectivenessResistance);
            PlayerPrefs.Save();
        }

        private void AddStatus(ref int remainingAddStatusPoint, ref int targetValueField) // return remaining point
        {
            if (remainingAddStatusPoint <= 0)
            {
                return;
            }

            int temp = Random.Range(1, remainingAddStatusPoint + 1);
            float maxValue = (float)gameConfig.AddStatusValuePerLevel / 8f; // have 8 fields

            if (temp > maxValue)
            {
                temp = (int)maxValue + Random.Range(0, 4);
            }

            targetValueField += temp;
            remainingAddStatusPoint -= temp;
        }
    }
}