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

        public abstract void Initialize();
    }
}