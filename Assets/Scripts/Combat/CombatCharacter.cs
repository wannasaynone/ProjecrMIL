using System.Collections;
using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatCharacter : MonoBehaviour
    {
        [SerializeField] private Animator playerCharacterAnimator;

        private void Start()
        {
            playerCharacterAnimator.Play("Idle");
            EventBus.Subscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
        }

        private void OnAttackButtonPressed(OnAttackButtonPressed e)
        {
            if (playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
                return;

            playerCharacterAnimator.Play(e.attackName, 0, 0f);
        }

        private void Update()
        {
            if (playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack04")
                && playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                playerCharacterAnimator.Play("IdleTransition");
                transform.position += 0.55f * Vector3.right;
            }

            if (playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleTransition"))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), 0.01f);
                if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) < 0.01f)
                {
                    transform.position = new Vector3(0, 0, 0);
                    playerCharacterAnimator.Play("Idle");
                }
            }
        }
    }
}