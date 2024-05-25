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

        private int comboCount = 0;

        private void OnAttackButtonPressed(OnAttackButtonPressed e)
        {
            if (playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
                return;

            // if ((comboCount == 0 && e.attackName == "ComboAttack01")
            //     || (comboCount == 1 && e.attackName == "ComboAttack02")
            //     || (comboCount == 2 && e.attackName == "ComboAttack03"))
            // {
            //     comboCount++;
            // }
            // else
            // {
            //     comboCount = 0;
            // }

            playerCharacterAnimator.Play(e.attackName, 0, 0f);
        }

        private void Update()
        {
            if (playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack04")
                && playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                transform.position += new Vector3(1f, 0, 0);
                playerCharacterAnimator.Play("Idle");
            }

            // if (playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("ComboAttack03")
            //     && playerCharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f
            //     && comboCount == 3)
            // {
            //     playerCharacterAnimator.Play("ComboAttack04", 0, 0f);
            //     comboCount = 0;
            // }
        }
    }
}