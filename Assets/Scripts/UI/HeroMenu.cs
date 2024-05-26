using System.Collections;
using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.UI
{
    public class HeroMenu : UIBase
    {
        [SerializeField] private TMPro.TextMeshProUGUI levelText;
        [SerializeField] private TMPro.TextMeshProUGUI expText;
        [SerializeField] private UnityEngine.UI.Slider expSlider;
        [SerializeField] private GameObject levelUpHintRoot;
        [SerializeField] private GameObject levelUpButtonRoot;
        [SerializeField] private TMPro.TextMeshProUGUI hpText;
        [SerializeField] private TMPro.TextMeshProUGUI defenseText;
        [SerializeField] private TMPro.TextMeshProUGUI attackText;
        [SerializeField] private TMPro.TextMeshProUGUI speedText;
        [SerializeField] private TMPro.TextMeshProUGUI criticalText;
        [SerializeField] private TMPro.TextMeshProUGUI criticalResistanceText;
        [SerializeField] private TMPro.TextMeshProUGUI effectivenessText;
        [SerializeField] private TMPro.TextMeshProUGUI effectivenessResistanceText;

        public override void Initialize()
        {
            EventBus.Subscribe<OnPlayerValueUpdated>(OnPlayerValueUpdated);
            EventBus.Subscribe<OnExpValueUpdated>(OnExpValueUpdated);
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnPlayerValueUpdated(OnPlayerValueUpdated e)
        {
            levelText.text = e.level.ToString();
            expText.text = e.exp.ToString() + "/" + e.requireExp.ToString();
            expSlider.value = (float)e.exp / e.requireExp;
            hpText.text = e.maxHP.ToString();
            defenseText.text = e.defense.ToString();
            attackText.text = e.attack.ToString();
            speedText.text = e.speed.ToString();
            criticalText.text = e.critical.ToString();
            criticalResistanceText.text = e.criticalResistance.ToString();
            effectivenessText.text = e.effectiveness.ToString();
            effectivenessResistanceText.text = e.effectivenessResistance.ToString();
            levelUpButtonRoot.SetActive(e.exp >= e.requireExp);
            levelUpHintRoot.SetActive(e.exp >= e.requireExp);
        }

        private void OnExpValueUpdated(OnExpValueUpdated e)
        {
            expText.text = e.newValue.ToString() + "/" + e.requireExp.ToString();
            expSlider.value = (float)e.newValue / e.requireExp;
            levelUpButtonRoot.SetActive(e.newValue >= e.requireExp);
            levelUpHintRoot.SetActive(e.newValue >= e.requireExp);
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            levelText.text = e.currentLevel.ToString();
            expSlider.value = (float)e.currentExp / e.requireExp;
            expText.text = e.currentExp.ToString() + "/" + e.requireExp.ToString();
            levelUpButtonRoot.SetActive(e.currentExp >= e.requireExp);
            levelUpHintRoot.SetActive(e.currentExp >= e.requireExp);
        }

        public void Button_LevelUp()
        {
            EventBus.Publish(new OnTryLevelUpCalled { tryAddLevel = 1 });
        }
    }
}
