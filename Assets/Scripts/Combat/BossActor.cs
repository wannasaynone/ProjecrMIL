using System.Collections;
using DG.Tweening;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class BossActor : CombatActor
    {
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private DamageNumberObject damageNumberObjectPrefab;
        [SerializeField] private GameObject hitEffectPrefab;

        private SpriteRenderer[] spriteRenderers;

        private enum BossState
        {
            Idle,
            GotHit,
            PrepareAttack,
            Attack,
            Attacked,
            PrepareSkill,
            SkillCasted,
            Dead,
            Pause
        }

        private BossState currentState = BossState.Idle;
        private float timer = 0f;

        protected override void OnInitialized()
        {
            timer = 1f;
            EventBus.Subscribe<OnDamageCalculated>(OnDamageCalculated);
            EventBus.Subscribe<OnAnyActorGotBlocked>(OnAnyActorGotBlocked);
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        protected override void OnDisposed()
        {
            EventBus.Unsubscribe<OnDamageCalculated>(OnDamageCalculated);
            EventBus.Unsubscribe<OnAnyActorGotBlocked>(OnAnyActorGotBlocked);
        }

        private void OnAnyActorGotBlocked(OnAnyActorGotBlocked e)
        {
            if (e.gotBlockedActorInstanceID == GetInstanceID())
            {
                transform.DOMove(transform.position + Vector3.right * Random.Range(1f, 1.5f), 0.15f).OnComplete(() =>
                {

                });
            }
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

        private void OnDamageCalculated(OnDamageCalculated e)
        {
            if (e.targetActorInstanceID == GetInstanceID())
            {
                StartCoroutine(IEApplyDamage(e));
            }
        }

        private void SetColor(Color color)
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.name == "Shadow" || spriteRenderer.name == "Front")
                    continue;

                spriteRenderer.color = color;
            }
        }

        private Color GetColor()
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.name == "Shadow" || spriteRenderer.name == "Front")
                    continue;

                return spriteRenderer.color;
            }

            return Color.white;
        }


        private IEnumerator IEApplyDamage(OnDamageCalculated e)
        {
            bool isBigAttack = currentState == BossState.Pause;

            DOTween.To(() => GetColor(), SetColor, Color.red, 0.15f);

            transform.DOMove(transform.position + Vector3.right * Random.Range(1f, 1.5f), 0.15f);

            GameObject cloneHitEffect = Instantiate(hitEffectPrefab, e.hitPosition, Quaternion.identity);
            Destroy(cloneHitEffect, 1f);

            EventBus.Publish(new OnAnyActorGotHit
            {
                attackerActorInstanceID = e.attackerActorInstanceID,
                targetActorInstanceID = e.targetActorInstanceID,
                hitPosition = e.hitPosition,
                damage = e.damage
            });

            DamageNumberObject damageNumberObject = Instantiate(damageNumberObjectPrefab, e.hitPosition + Vector3.up, Quaternion.identity);
            damageNumberObject.SetDamage(e.damage);
            damageNumberObject.ShowAnimation(DamageNumberObject.AnimationType.UpFade);
            Destroy(damageNumberObject.gameObject, 1f);

            if (isBigAttack)
            {
                PlayAnimation("3_Debuff_Stun", 1.5f);
            }

            yield return new WaitForSeconds(0.15f);

            DOTween.To(() => GetColor(), SetColor, Color.white, 0.15f);

            yield return new WaitForSeconds(0.15f);

            Info.currentHP -= e.damage;

            if (Info.currentHP <= 0)
            {
                currentState = BossState.Dead;
                PlayAnimation("4_Death");
            }
            else if (isBigAttack)
            {
                timer = 0.5f;
                currentState = BossState.Idle;
                PlayAnimation("0_idle");
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
                        PlayAnimation("2_Attack_Normal");
                        StartCoroutine(IEDashToEnemy(playerActor));
                        timer = 1f;
                    }
                    break;
                case BossState.PrepareAttack:

                    if (IsPlaying("2_Attack_Normal") && GetNormalizedTime() >= 0.31f)
                    {
                        PauseAnimation();
                    }

                    if (timer > 0f || Vector3.Distance(transform.position, playerActor.transform.position) > attackRange)
                    {
                        timer -= Time.deltaTime;
                        if (timer <= 0f)
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

                        if (Vector3.Distance(transform.position, playerActor.transform.position) <= attackRange)
                        {
                            EventBus.Publish(new OnAttackCasted
                            {
                                attackerActorInstanceID = GetInstanceID(),
                                targetActorInstanceID = playerActor.GetInstanceID(),
                                hitPosition = (playerActor.transform.position + transform.position) / 2f + new Vector3(Random.Range(-0.3f, 0.3f), 1f + Random.Range(-0.2f, 0.2f), -5f)
                            });
                        }
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

        protected override void OnPaused()
        {
            currentState = BossState.Pause;
        }
    }
}