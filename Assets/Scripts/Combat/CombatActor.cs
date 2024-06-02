using System.Collections;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public abstract class CombatActor : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private string defaultStateName = "Idle";

        public class ActorInfo
        {
            public enum Camp
            {
                Player,
                Enemy
            }
            public class Templete
            {
                public int maxHP;
                public int attack;
                public int defense;
                public int speed;
                public int critical;
                public int criticalResistance;
                public int effectiveness;
                public int effectivenessResistance;
                public Camp camp;
            }
            public int MaxHP { get; private set; }
            public int Attack { get; private set; }
            public int Defense { get; private set; }
            public int Speed { get; private set; }
            public int Critical { get; private set; }
            public int CriticalResistance { get; private set; }
            public int Effectiveness { get; private set; }
            public int EffectivenessResistance { get; private set; }
            public Camp ActorCamp { get; private set; }
            public ActorInfo(Templete templete)
            {
                MaxHP = templete.maxHP;
                Attack = templete.attack;
                Defense = templete.defense;
                Speed = templete.speed;
                Critical = templete.critical;
                CriticalResistance = templete.criticalResistance;
                Effectiveness = templete.effectiveness;
                EffectivenessResistance = templete.effectivenessResistance;
                ActorCamp = templete.camp;
            }
        }
        public ActorInfo Info { get; private set; }

        private void Start()
        {
            characterAnimator.Play(defaultStateName, 0, 0f);
        }

        protected void PlayAnimation(string animationName, float speed = 1f)
        {
            characterAnimator.speed = speed;
            characterAnimator.Play(animationName, 0, 0f);
        }

        protected void PlayAnimationAndStop(string animationName, float stopAtNormalizedTime, float speed = 1f)
        {
            characterAnimator.speed = speed;
            characterAnimator.Play(animationName, 0, 0f);
            StartCoroutine(IEStopAnimation(animationName, stopAtNormalizedTime));
        }

        private IEnumerator IEStopAnimation(string animationName, float stopAtNormalizedTime)
        {
            while (IsPlaying(animationName) && GetNormalizedTime() < stopAtNormalizedTime)
            {
                yield return null;
            }

            orignalSpeed = characterAnimator.speed;
            characterAnimator.speed = 0;
        }

        private float orignalSpeed = 1f;

        protected void ResumeAnimation()
        {
            characterAnimator.speed = orignalSpeed;
        }

        protected bool IsPlaying(string stateName)
        {
            return characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        protected float GetNormalizedTime()
        {
            return characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public void Initialize(ActorInfo actorInfo)
        {
            Info = actorInfo;
            CombatActorContainer.AddActor(this);
            OnInitialized();
        }

        public void Dispose()
        {
            CombatActorContainer.RemoveActor(this);
            OnDisposed();
            Destroy(gameObject);
        }

        protected abstract void OnInitialized();
        protected abstract void OnDisposed();
    }
}