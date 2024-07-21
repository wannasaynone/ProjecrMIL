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

        private KahaGameCore.GameData.Implemented.JsonSaveDataHandler jsonSaveDataHandler;

        public void Initail()
        {
            jsonSaveDataHandler = new KahaGameCore.GameData.Implemented.JsonSaveDataHandler
                    (
                        new KahaGameCore.GameData.Implemented.GameStaticDataSerializer(),
                        new KahaGameCore.GameData.Implemented.GameStaticDataDeserializer()
                    );

            saveData = jsonSaveDataHandler.LoadSave<SaveData>();
            if (saveData == default)
            {
                saveData = new SaveData
                {
                    level = 1,
                    gold = 0,
                    maxHP = 100,
                    attack = 100,
                    defense = 100,
                    speed = 100,
                    critical = 0,
                    criticalResistance = 0,
                    effectiveness = 0,
                    effectivenessResistance = 0
                };
                jsonSaveDataHandler.Save(saveData);
            }

            EventBus.Subscribe<OnAdventureEventCreated_Gold>(OnAdventureEventCreated_Gold);
            EventBus.Subscribe<OnTryLevelUpCalled>(OnTryLevelUpCalled);

            EventBus.Publish(new OnPlayerValueUpdated
            {
                level = saveData.level,
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


        private void OnAdventureEventCreated_Gold(OnAdventureEventCreated_Gold created)
        {
            saveData.gold += created.addGold;
            SaveCurrent();
            EventBus.Publish(new OnGoldValueUpdated
            {
                oldValue = saveData.gold - created.addGold,
                addValue = created.addGold,
                newValue = saveData.gold,
                requireExp = GetRequireWithCurrentLevel(),
                currentLevel = saveData.level
            });
        }

        private void OnTryLevelUpCalled(OnTryLevelUpCalled e)
        {
            int oldLevel = saveData.level;
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

            for (int i = 0; i < e.tryAddLevel; i++)
            {
                if (saveData.gold >= GetRequireWithCurrentLevel())
                {
                    saveData.gold -= GetRequireWithCurrentLevel();
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
                    currentGold = saveData.gold,
                    requireExp = GetRequireWithCurrentLevel(),
                    beforeHP = fields[0],
                    beforeDefense = fields[1],
                    beforeAttack = fields[2],
                    beforeSpeed = fields[3],
                    beforeCritical = fields[4],
                    beforeCriticalResistance = fields[5],
                    beforeEffectiveness = fields[6],
                    beforeEffectivenessResistance = fields[7],
                    afterHP = saveData.maxHP,
                    afterDefense = saveData.defense,
                    afterAttack = saveData.attack,
                    afterSpeed = saveData.speed,
                    afterCritical = saveData.critical,
                    afterCriticalResistance = saveData.criticalResistance,
                    afterEffectiveness = saveData.effectiveness,
                    afterEffectivenessResistance = saveData.effectivenessResistance
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
            jsonSaveDataHandler.Save(saveData);
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