using System.Collections;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public abstract class CombatActor : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private string defaultStateName = "Idle";
        [SerializeField] private float width = 1f;

        public float GetBound()
        {
            if (Info == null)
                return transform.position.x;

            if (Info.ActorCamp == ActorInfo.Camp.Player)
                return transform.position.x + width / 2;
            else
                return transform.position.x - width / 2;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(GetBound(), transform.position.y + 0.5f, 0), new Vector3(transform.position.x, transform.position.y + 0.5f, 0));
        }

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
            public int currentHP;
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
                currentHP = MaxHP;
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

        protected Transform GetAnimatorRoot()
        {
            return characterAnimator.transform;
        }

        protected void PlayAnimation(string animationName, float speed = 1f)
        {
            characterAnimator.speed = speed;
            characterAnimator.Play(animationName, 0, 0f);
        }

        protected void PauseAnimation()
        {
            characterAnimator.speed = 0;
        }

        protected void ResumeAnimation(float speed = 1f)
        {
            characterAnimator.speed = speed;
        }

        protected bool IsPlaying(string stateName)
        {
            return characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        protected float GetNormalizedTime()
        {
            return characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public void Pause()
        {
            PauseAnimation();
            OnPaused();
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
        protected abstract void OnPaused();
    }
}