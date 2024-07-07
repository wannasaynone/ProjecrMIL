using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using ProjectMIL.GameEvent;

namespace ProjectMIL.UI
{
    public class EventPopup_GetExp : UIBase
    {
        [SerializeField] private GameObject infoPopupRoot;
        [SerializeField] private TMPro.TextMeshProUGUI titleText;
        [SerializeField] private TMPro.TextMeshProUGUI descText;
        [SerializeField] private GameObject particleImageRoot;
        [SerializeField] private UnityEvent onEnabled; // for workaround with ParticleImage
        [SerializeField] private UnityEvent onPlayParticleCalled; // for workaround with ParticleImage


        public override void Initialize(Utlity.ContextHandler contextHandler)
        {
            EventBus.Subscribe<OnAdventureProgressBarAnimationEnded>(OnAdventureProgressBarAnimationEnded);
        }

        private void OnDisable()
        {
            EventBus.Publish(new OnAdventureEventResultPanelDisabled());
        }

        private void OnAdventureProgressBarAnimationEnded(OnAdventureProgressBarAnimationEnded ended)
        {
        }
    }
}