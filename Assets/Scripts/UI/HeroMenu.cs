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

        public override void Initialize(Utlity.ContextHandler contextHandler)
        {
            EventBus.Subscribe<OnPlayerValueUpdated>(OnPlayerValueUpdated);
            EventBus.Subscribe<OnGoldValueUpdated>(OnGoldValueUpdated);
            EventBus.Subscribe<OnLevelUpdated>(OnLevelUpdated);
        }

        private void OnPlayerValueUpdated(OnPlayerValueUpdated e)
        {
            levelText.text = e.level.ToString();
            expText.text = e.gold.ToString() + "/" + e.requireExp.ToString();
            expSlider.value = (float)e.gold / e.requireExp;
            hpText.text = e.maxHP.ToString();
            defenseText.text = e.defense.ToString();
            attackText.text = e.attack.ToString();
            speedText.text = e.speed.ToString();
            criticalText.text = e.critical.ToString();
            criticalResistanceText.text = e.criticalResistance.ToString();
            effectivenessText.text = e.effectiveness.ToString();
            effectivenessResistanceText.text = e.effectivenessResistance.ToString();
            levelUpButtonRoot.SetActive(e.gold >= e.requireExp);
            levelUpHintRoot.SetActive(e.gold >= e.requireExp);
        }

        private void OnGoldValueUpdated(OnGoldValueUpdated e)
        {
            expText.text = e.newValue.ToString() + "/" + e.requireExp.ToString();
            expSlider.value = (float)e.newValue / e.requireExp;
            levelUpButtonRoot.SetActive(e.newValue >= e.requireExp);
            levelUpHintRoot.SetActive(e.newValue >= e.requireExp);
        }

        private void OnLevelUpdated(OnLevelUpdated e)
        {
            levelText.text = e.currentLevel.ToString();
            expSlider.value = (float)e.currentGold / e.requireExp;
            expText.text = e.currentGold.ToString() + "/" + e.requireExp.ToString();
            levelUpButtonRoot.SetActive(e.currentGold >= e.requireExp);
            levelUpHintRoot.SetActive(e.currentGold >= e.requireExp);
        }

        public void Button_LevelUp()
        {
            EventBus.Publish(new OnTryLevelUpCalled { tryAddLevel = 1 });
        }
    }
}
