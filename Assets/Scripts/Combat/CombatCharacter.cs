using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatCharacter : MonoBehaviour
    {
        [SerializeField] private Animator playerCharacterAnimator;

        private int attackIndex = 1;

        private void Start()
        {
            playerCharacterAnimator.Play("Idle");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                playerCharacterAnimator.Play("ComboAttack0" + attackIndex);
                attackIndex++;
                if (attackIndex > 4)
                {
                    attackIndex = 1;
                }
            }

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