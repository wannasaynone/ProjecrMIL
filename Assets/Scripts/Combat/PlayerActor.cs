using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        private SpriteRenderer[] spriteRenderers;

        protected override void OnInitialized()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            EventBus.Subscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
            EventBus.Subscribe<OnDamageCalculated>(OnDamageCalculated);
            spriteRenderer.material = new Material(spriteRenderer.material);
        }

        protected override void OnDisposed()
        {
            EventBus.Unsubscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
            EventBus.Unsubscribe<OnDamageCalculated>(OnDamageCalculated);
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
                PlayAnimationAndStop(e.attackName, 0f, 2f);
                StartCoroutine(IEDashToEnemy(enemyActor));
            }
        }

        private void OnDamageCalculated(OnDamageCalculated e)
        {
            if (e.targetActorInstanceID == GetInstanceID())
            {
                StartCoroutine(IEGotHit(e));
            }
        }

        private bool isShowingHitEffect = false;
        private IEnumerator IEGotHit(OnDamageCalculated e)
        {
            if (isShowingHitEffect)
                yield break;

            isShowingHitEffect = true;
            DOTween.To(() => GetColor(), SetColor, Color.red, 0.15f);

            EventBus.Publish(new OnGotHit
            {
                attackerActorInstanceID = e.attackerActorInstanceID,
                targetActorInstanceID = e.targetActorInstanceID,
                damage = e.damage,
                hitPosition = e.hitPosition
            });

            yield return new WaitForSeconds(0.15f);
            DOTween.To(() => GetColor(), SetColor, Color.white, 0.15f);
            isShowingHitEffect = false;
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

        private IEnumerator IEDashToEnemy(CombatActor enemyActor)
        {
            float motionBlur = 0f;

            float stopRange = 2f;
            for (int attackInfoIndex = 0; attackInfoIndex < attackInfos.Length; attackInfoIndex++)
            {
                if (currentAttackName == attackInfos[attackInfoIndex].attackName)
                {
                    stopRange = attackInfos[attackInfoIndex].attackRange;
                    break;
                }
            }

            while (Vector3.Distance(transform.position, enemyActor.transform.position) > stopRange)
            {
                float moveSpeed = 25f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, enemyActor.transform.position, moveSpeed);
                motionBlur = Mathf.Lerp(motionBlur, 1f, moveSpeed);
                speedLineEffectRoot.SetActive(true);
                yield return null;
            }
            speedLineEffectRoot.SetActive(false);
            ResumeAnimation();
        }

        private void Update()
        {
            if (IsPlaying("ComboAttack04") && GetNormalizedTime() >= 1f)
            {
                transform.position += new Vector3(1f, 0, 0);
                PlayAnimation("Idle");
            }
        }

        private void LateUpdate() // for detecting animation time
        {
            if (IsPlaying("Idle"))
                return;

            if (isAttacked)
                return;

            for (int attackInfoIndex = 0; attackInfoIndex < attackInfos.Length; attackInfoIndex++)
            {
                if (currentAttackName == attackInfos[attackInfoIndex].attackName && GetNormalizedTime() >= attackInfos[attackInfoIndex].attackStartNormalizedTime)
                {
                    List<CombatActor> enemyActors = CombatActorContainer.GetAllActorInRange(ActorInfo.Camp.Enemy, transform.position, attackInfos[attackInfoIndex].attackRange);

                    for (int enemyActorIndex = 0; enemyActorIndex < enemyActors.Count; enemyActorIndex++)
                    {
                        CombatActor enemyActor = enemyActors[enemyActorIndex];

                        EventBus.Publish(new OnAttackCasted
                        {
                            attackerActorInstanceID = GetInstanceID(),
                            targetActorInstanceID = enemyActor.GetInstanceID(),
                            hitPosition = (enemyActor.transform.position + transform.position) / 2f + new Vector3(Random.Range(-0.3f, 0.3f), 1f + Random.Range(-0.2f, 0.2f), -5f)
                        });
                    }
                    isAttacked = true;
                    break;
                }
            }
        }
    }
}