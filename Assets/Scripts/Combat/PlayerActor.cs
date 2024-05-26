using System.Collections;
using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class PlayerActor : CombatActor
    {
        [System.Serializable]
        private class AttackInfo
        {
            public string attackName;
            public float attackStartNormalizedTime;
            public float attackRange;
        }

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject speedLineEffectRoot;
        [SerializeField] private AttackInfo[] attackInfos;

        private bool isAttacked = false;
        private string currentAttackName;

        protected override void OnInitialized()
        {
            EventBus.Subscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
            spriteRenderer.material = new Material(spriteRenderer.material);
        }

        private void OnAttackButtonPressed(OnAttackButtonPressed e)
        {
            if (!IsPlaying("Idle") && GetNormalizedTime() < 0.9f)
                return;

            isAttacked = false;
            currentAttackName = e.attackName;

            CombatActor enemyActor = CombatActorContainer.GetCloestUnitByCamp(ActorInfo.Camp.Enemy, transform.position);
            if (enemyActor == null)
            {
                PlayAnimation(e.attackName, 2f);
            }
            else
            {
                PlayAnimationAndStop(e.attackName, 0.01f, 2f);
                StartCoroutine(IEDashToEnemy(enemyActor));
            }
        }

        private IEnumerator IEDashToEnemy(CombatActor enemyActor)
        {
            float motionBlur = 0f;
            while (Vector3.Distance(transform.position, enemyActor.transform.position) > 2f)
            {
                float moveSpeed = 25f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, enemyActor.transform.position, moveSpeed);
                motionBlur = Mathf.Lerp(motionBlur, 1f, moveSpeed);
                spriteRenderer.material.SetFloat("_MotionBlurDist", motionBlur);
                speedLineEffectRoot.SetActive(true);
                yield return null;
            }
            speedLineEffectRoot.SetActive(false);
            spriteRenderer.material.SetFloat("_MotionBlurDist", 0f);
            ResumeAnimation();
        }

        private void Update()
        {
            if (IsPlaying("ComboAttack04") && GetNormalizedTime() >= 1f)
            {
                transform.position += new Vector3(1f, 0, 0);
                PlayAnimation("Idle");
            }

            if (IsPlaying("Idle"))
                return;

            if (isAttacked)
                return;

            for (int attackInfoIndex = 0; attackInfoIndex < attackInfos.Length; attackInfoIndex++)
            {
                if (currentAttackName == attackInfos[attackInfoIndex].attackName && GetNormalizedTime() >= attackInfos[attackInfoIndex].attackStartNormalizedTime)
                {
                    List<CombatActor> enemyUnits = CombatActorContainer.GetAllUnitInRange(ActorInfo.Camp.Enemy, transform.position, attackInfos[attackInfoIndex].attackRange);

                    for (int enemyActorIndex = 0; enemyActorIndex < enemyUnits.Count; enemyActorIndex++)
                    {
                        CombatActor enemyActor = enemyUnits[enemyActorIndex];
                        EventBus.Publish(new OnStartToHit
                        {
                            attackerActorInstanceID = GetInstanceID(),
                            targetActorInstanceID = enemyActor.GetInstanceID()
                        });
                    }
                    isAttacked = true;
                    break;
                }
            }
        }
    }
}