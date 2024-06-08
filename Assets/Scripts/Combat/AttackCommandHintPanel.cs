using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectMIL.Combat
{
    public class AttackCommandHintPanel : MonoBehaviour
    {
        [SerializeField] private Image hintImagePrefab;
        [SerializeField] private Transform hintImageRoot;

        [System.Serializable]
        private class HintImageInfo
        {
            public string command;
            public Sprite sprite;
        }

        [SerializeField] private HintImageInfo[] hintImageInfos;

        private List<Image> cloneHintImage = new List<Image>();

        public void StartListening()
        {
            EventBus.Subscribe<OnAttackCommandsCreated>(OnAttackCommandsCreated);
            EventBus.Subscribe<OnAttackCommandMatchedWithIndex>(OnAttackCommandMatchedWithIndex);
            EventBus.Subscribe<OnAttackCommandFailed>(OnAttackCommandFailed);
            EventBus.Subscribe<OnAttackCommandAllMatched>(OnAttackCommandAllMatched);
        }

        public void StopListening()
        {
            EventBus.Unsubscribe<OnAttackCommandsCreated>(OnAttackCommandsCreated);
            EventBus.Unsubscribe<OnAttackCommandMatchedWithIndex>(OnAttackCommandMatchedWithIndex);
            EventBus.Unsubscribe<OnAttackCommandFailed>(OnAttackCommandFailed);
            EventBus.Unsubscribe<OnAttackCommandAllMatched>(OnAttackCommandAllMatched);
        }

        private void OnAttackCommandsCreated(OnAttackCommandsCreated e)
        {
            ShowWith(e.attackCommands);
        }

        private void OnAttackCommandMatchedWithIndex(OnAttackCommandMatchedWithIndex e)
        {
            if (e.index < cloneHintImage.Count)
            {
                cloneHintImage[e.index].color = Color.gray;
            }
        }

        private void OnAttackCommandAllMatched(OnAttackCommandAllMatched e)
        {
            gameObject.SetActive(false);
        }

        private void ShowWith(string[] commands)
        {
            foreach (var image in cloneHintImage)
            {
                Destroy(image.gameObject);
            }
            cloneHintImage.Clear();

            foreach (var command in commands)
            {
                var hintImageInfo = System.Array.Find(hintImageInfos, x => x.command == command);
                if (hintImageInfo == null)
                    continue;

                var clone = Instantiate(hintImagePrefab, hintImageRoot);
                clone.sprite = hintImageInfo.sprite;
                cloneHintImage.Add(clone);
            }

            gameObject.SetActive(true);
        }

        private void OnAttackCommandFailed(OnAttackCommandFailed e)
        {
            for (int i = 0; i < cloneHintImage.Count; i++)
            {
                cloneHintImage[i].color = Color.white;
            }
        }
    }
}