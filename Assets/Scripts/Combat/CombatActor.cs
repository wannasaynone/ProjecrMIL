using System;
using System.Collections;
using System.Threading;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public abstract class CombatActor : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private string defaultStateName = "Idle";

        private void Start()
        {
            characterAnimator.Play(defaultStateName, 0, 0f);
        }

        protected void PlayAnimation(string animationName)
        {
            characterAnimator.Play(animationName, 0, 0f);
        }

        protected void PlayAnimationAndStop(string animationName, float stopAtNormalizedTime)
        {
            characterAnimator.Play(animationName, 0, 0f);
            StartCoroutine(IEStopAnimation(animationName, stopAtNormalizedTime));
        }

        private IEnumerator IEStopAnimation(string animationName, float stopAtNormalizedTime)
        {
            while (IsPlaying(animationName) && GetNormalizedTime() < stopAtNormalizedTime)
            {
                yield return null;
            }

            characterAnimator.speed = 0;
        }

        protected void ResumeAnimation()
        {
            characterAnimator.speed = 1;
        }

        protected bool IsPlaying(string stateName)
        {
            return characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        protected float GetNormalizedTime()
        {
            return characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public abstract void Initialize();
    }
}