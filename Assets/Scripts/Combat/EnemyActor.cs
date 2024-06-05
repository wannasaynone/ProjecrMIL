using System.Collections;
using ProjectMIL.GameEvent;
using UnityEngine;
using DG.Tweening;

namespace ProjectMIL.Combat
{
    public class EnemyActor : CombatActor
    {
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float moveSpeed = 1f;

        private SpriteRenderer[] spriteRenderers;

        protected override void OnInitialized()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            EventBus.Subscribe<OnDamageCalculated>(OnDamageCalculated);
        }

        protected override void OnDisposed()
        {
            EventBus.Unsubscribe<OnDamageCalculated>(OnDamageCalculated);
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
            if (aiState == AIState.GotHit)
                yield break;

            aiState = AIState.GotHit;

            DOTween.To(() => GetColor(), SetColor, Color.red, 0.15f);
            PlayAnimation("3_Debuff_Stun", 1.5f);
            transform.DOMove(transform.position + Vector3.right * Random.Range(1f, 3f), 0.15f);

            GameObject cloneHitEffect = Instantiate(hitEffectPrefab, e.hitPosition, Quaternion.identity);
            Destroy(cloneHitEffect, 1f);

            EventBus.Publish(new OnAnyActorGotHit
            {
                attackerActorInstanceID = e.attackerActorInstanceID,
                targetActorInstanceID = e.targetActorInstanceID,
                hitPosition = e.hitPosition,
                damage = e.damage
            });

            yield return new WaitForSeconds(0.15f);

            DOTween.To(() => GetColor(), SetColor, Color.white, 0.15f);

            yield return new WaitForSeconds(0.15f);

            PlayAnimation("0_idle", 1f);
            aiState = AIState.Idle;
        }

        private enum AIState
        {
            Idle,
            Attack,
            Walk,
            Attaking,
            Attacked,
            GotHit,
            Dead
        }

        private AIState aiState = AIState.Idle;
        private CombatActor playerActor;

        private void Update()
        {
            switch (aiState)
            {
                case AIState.Idle:

                    playerActor = CombatActorContainer.GetCloestUnitByCamp(ActorInfo.Camp.Player, transform.position);
                    if (playerActor != null)
                    {
                        if (Vector3.Distance(playerActor.transform.position, transform.position) <= attackRange)
                        {
                            aiState = AIState.Attack;
                        }
                        else
                        {
                            PlayAnimation("1_Run");
                            aiState = AIState.Walk;
                        }
                    }
                    break;
                case AIState.Walk:
                    if (playerActor == null)
                    {
                        aiState = AIState.Idle;
                        PlayAnimation("0_idle");
                        return;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, playerActor.transform.position, moveSpeed * Time.deltaTime);
                    if (Vector3.Distance(playerActor.transform.position, transform.position) <= attackRange)
                    {
                        aiState = AIState.Attack;
                    }
                    break;
                case AIState.Attack:
                    PlayAnimation("2_Attack_Normal");
                    aiState = AIState.Attaking;
                    break;
                case AIState.Attaking:
                    if (IsPlaying("2_Attack_Normal") && GetNormalizedTime() >= 0.5f)
                    {
                        playerActor = CombatActorContainer.GetCloestUnitByCamp(ActorInfo.Camp.Player, transform.position);
                        if (playerActor != null && Vector3.Distance(playerActor.transform.position, transform.position) <= attackRange)
                        {
                            EventBus.Publish(new OnAttackCasted
                            {
                                attackerActorInstanceID = GetInstanceID(),
                                targetActorInstanceID = playerActor.GetInstanceID(),
                                hitPosition = (playerActor.transform.position + transform.position) / 2f + new Vector3(Random.Range(-0.3f, 0.3f), 1f + Random.Range(-0.2f, 0.2f), -5f)
                            });
                        }
                        aiState = AIState.Attacked;
                    }
                    break;
                case AIState.Attacked:
                    if (IsPlaying("2_Attack_Normal") && GetNormalizedTime() >= 1f)
                    {
                        aiState = AIState.Idle;
                        PlayAnimation("0_idle");
                    }
                    break;
                case AIState.GotHit:
                    break;
                case AIState.Dead:
                    break;
            }
        }
    }
}