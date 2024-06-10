using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class AttackCommandHandler
    {
        private readonly string[] attackNames;
        private readonly string returnAttackName;

        private List<string> commandList;
        private int matchedTimes = 0;
        private bool isInCooldown = false;

        public AttackCommandHandler(string[] attackNames, string returnAttackName)
        {
            this.attackNames = new string[attackNames.Length];
            for (int i = 0; i < attackNames.Length; i++)
            {
                this.attackNames[i] = attackNames[i];
            }
            this.returnAttackName = returnAttackName;
        }

        public void StartListening()
        {
            EventBus.Subscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
        }

        public void StopListening()
        {
            EventBus.Unsubscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
        }

        private void OnAttackButtonPressed(OnAttackButtonPressed e)
        {
            if (isInCooldown)
                return;

            if (commandList == null || commandList.Count == 0)
            {
                if (Random.Range(0f, 100f) <= 30f)
                {
                    commandList = new List<string>();
                    List<string> temp = new List<string>(attackNames);
                    for (int i = 0; i < attackNames.Length; i++)
                    {
                        int randomIndex = Random.Range(0, temp.Count);
                        commandList.Add(temp[randomIndex]);
                        temp.RemoveAt(randomIndex);
                    }

                    matchedTimes = 0;
                    EventBus.Publish(new OnAttackCommandsCreated
                    {
                        attackCommands = new List<string>(commandList).ToArray()
                    });
                }

            }
            else
            {
                if (e.attackName == commandList[matchedTimes])
                {
                    EventBus.Publish(new OnAttackCommandMatchedWithIndex
                    {
                        index = matchedTimes
                    });

                    matchedTimes++;

                    if (matchedTimes == commandList.Count)
                    {
                        isInCooldown = true;
                        StartResetCooldown();
                        EventBus.Publish(new OnAttackCommandAllMatched
                        {
                            returnAttackName = returnAttackName
                        });
                        commandList = null;
                    }
                }
                else
                {
                    EventBus.Publish(new OnAttackCommandFailed());
                    matchedTimes = 0;
                }
            }
        }

        private async void StartResetCooldown()
        {
            await System.Threading.Tasks.Task.Delay(10000);
            isInCooldown = false;
        }
    }
}