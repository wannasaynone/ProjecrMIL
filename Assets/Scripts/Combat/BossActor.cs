using System.Collections;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class BossActor : CombatActor
    {
        [SerializeField] private float attackRange = 2f;

        private enum BossState
        {
            Idle,
            PrepareAttack,
            Attack,
            Attacked,
            PrepareSkill,
            SkillCasted,
            Dead
        }

        private BossState currentState = BossState.Idle;
        private float timer = 0f;

        protected override void OnDisposed()
        {

        }

        protected override void OnInitialized()
        {
            timer = 1f;
        }

        private IEnumerator IEDashToEnemy(CombatActor playerActor)
        {
            while (Vector3.Distance(transform.position, playerActor.transform.position) > attackRange)
            {
                float moveSpeed = 5f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, playerActor.transform.position, moveSpeed);
                yield return null;
            }
        }

        private void LateUpdate()
        {
            CombatActor playerActor = CombatActorContainer.GetAnyActorByCamp(ActorInfo.Camp.Player);
            if (playerActor == null)
                return;

            switch (currentState)
            {
                case BossState.Idle:
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        currentState = BossState.PrepareAttack;
                        PlayAnimationAndStop("2_Attack_Normal", 0.31f);
                        StartCoroutine(IEDashToEnemy(playerActor));
                        timer = 1f;
                    }
                    break;
                case BossState.PrepareAttack:
                    if (timer > 0f || Vector3.Distance(transform.position, playerActor.transform.position) > attackRange)
                    {
                        timer -= Time.deltaTime;
                        if (timer <= 0f && Vector3.Distance(transform.position, playerActor.transform.position) <= attackRange)
                        {
                            currentState = BossState.Attack;
                            ResumeAnimation();
                        }
                    }
                    break;
                case BossState.Attack:
                    if (GetNormalizedTime() >= 0.64f)
                    {
                        currentState = BossState.Attacked;
                        PauseAnimation();
                        timer = 1f;
                    }
                    break;
                case BossState.Attacked:
                    if (timer <= 0f && GetNormalizedTime() >= 0.99f)
                    {
                        currentState = BossState.Idle;
                        PlayAnimation("0_idle");
                    }

                    if (timer > 0f)
                    {
                        timer -= Time.deltaTime;
                        if (timer <= 0f)
                        {
                            ResumeAnimation();
                        }
                    }
                    break;
                case BossState.PrepareSkill:
                    break;
                case BossState.SkillCasted:
                    break;
                case BossState.Dead:
                    break;
            }
        }
    }
}