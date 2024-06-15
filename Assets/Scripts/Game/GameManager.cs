using System.Collections;
using KahaGameCore.GameData.Implemented;
using ProjectMIL.Data;
using ProjectMIL.GameEvent;
using UnityEngine;
using DG.Tweening;

namespace ProjectMIL.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Combat.CombatManager combatManager;
        [SerializeField] private CanvasGroup blackScreenCanvasGroup;
        [SerializeField] private GameConfig gameConfig;

        private Adventure.AdventureManager adventureManager;
        private Utlity.ContextHandler contextHandler;
        private GameStaticDataManager gameStaticDataManager;
        private GameStaticDataDeserializer gameStaticDataDeserializer;

        private Player player;

        private int difficulty = -1;

        private void Awake()
        {
            gameStaticDataManager = new GameStaticDataManager();
            gameStaticDataDeserializer = new GameStaticDataDeserializer();
            gameStaticDataManager.Add<ExpData>(gameStaticDataDeserializer.Read<ExpData[]>(Resources.Load<TextAsset>("Data/ExpData").text));
            gameStaticDataManager.Add<ContextData>(gameStaticDataDeserializer.Read<ContextData[]>(Resources.Load<TextAsset>("Data/ContextData").text));

            contextHandler = new Utlity.ContextHandler(gameStaticDataManager);

            adventureManager = new Adventure.AdventureManager();
            adventureManager.Initialize(contextHandler);

            uiManager.Initialize();

            Element.UserInterfaceContextSetter[] userInterfaceContextSetters = FindObjectsOfType<Element.UserInterfaceContextSetter>(true);
            foreach (Element.UserInterfaceContextSetter userInterfaceContextSetter in userInterfaceContextSetters)
            {
                userInterfaceContextSetter.SetUp(contextHandler);
            }

            player = new Player(gameStaticDataManager.GetAllGameData<ExpData>(), gameConfig);
            player.Initail();

            EventBus.Subscribe<OnAdventureEventCreated_EncounterEnemy>(OnAdventureEventCreated_EncounterEnemy);
            EventBus.Subscribe<OnAdventureProgressBarAnimationEnded>(OnAdventureProgressBarAnimationEnded);
            EventBus.Subscribe<OnCombatStartCalled>(OnCombatStartCalled);
            EventBus.Subscribe<OnCombatEndCalled>(OnCombatEndCalled);
        }

        private void OnAdventureEventCreated_EncounterEnemy(OnAdventureEventCreated_EncounterEnemy e)
        {
            difficulty = e.difficulty;
        }

        private void OnAdventureProgressBarAnimationEnded(OnAdventureProgressBarAnimationEnded e)
        {
            if (difficulty == -1)
            {
                return;
            }

            SaveData saveData = player.GetSaveDataClone();
            EventBus.Publish(new OnCombatStartCalled
            {
                levelType = 0,
                difficulty = difficulty,
                maxHP = saveData.maxHP,
                attack = saveData.attack,
                defense = saveData.defense,
                speed = saveData.speed,
                critical = saveData.critical,
                criticalResistance = saveData.criticalResistance,
                effectiveness = saveData.effectiveness,
                effectivenessResistance = saveData.effectivenessResistance
            });

            difficulty = -1;
        }

        private void OnCombatStartCalled(OnCombatStartCalled e)
        {
            StartCoroutine(IEEnterCombatAnimation(e));
        }

        private IEnumerator IEEnterCombatAnimation(OnCombatStartCalled e)
        {
            blackScreenCanvasGroup.alpha = 0;
            blackScreenCanvasGroup.gameObject.SetActive(true);
            blackScreenCanvasGroup.DOFade(1f, 0.25f);

            yield return new WaitForSeconds(0.25f);

            uiManager.gameObject.SetActive(false);
            combatManager.CreateLevel(e); // TODO: use event to handle this flow

            yield return new WaitForSeconds(0.25f);

            blackScreenCanvasGroup.DOFade(0f, 0.25f);

            yield return new WaitForSeconds(0.25f);

            blackScreenCanvasGroup.gameObject.SetActive(false);
            combatManager.StartCurrentLevel();
        }

        private void OnCombatEndCalled(OnCombatEndCalled e)
        {
            StartCoroutine(IEExitCombatAnimation());
        }

        private IEnumerator IEExitCombatAnimation()
        {
            blackScreenCanvasGroup.alpha = 0;
            blackScreenCanvasGroup.gameObject.SetActive(true);
            blackScreenCanvasGroup.DOFade(1f, 0.5f);

            yield return new WaitForSeconds(0.5f);

            combatManager.EndCombat();
            uiManager.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.15f);

            blackScreenCanvasGroup.DOFade(0f, 0.5f);

            yield return new WaitForSeconds(0.5f);

            blackScreenCanvasGroup.gameObject.SetActive(false);
        }
    }
}