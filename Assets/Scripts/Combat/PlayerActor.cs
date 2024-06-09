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
        [SerializeField] private Material waveMaterial;
        [SerializeField] private GameObject speedLineEffectRoot;
        [SerializeField] private GameObject blockEffectPrefab;
        [SerializeField] private DamageNumberObject blockObjectPrefab;
        [SerializeField] private DamageNumberObject damageNumberObjectPrefab;
        [SerializeField] private SpriteRenderer blackSpriteRenderer;
        [SerializeField] private GameObject chargeEffectRoot;
        [SerializeField] private GameObject chargeDoneEffect;
        [SerializeField] private SpriteRenderer chargeDoneSpriteRenderer;
        [SerializeField] private ParticleSystem attack04Effect;
        [SerializeField] private AttackInfo[] attackInfos;

        private bool isAttacked = false;
        private bool isDead = false;
        private string currentAttackName;
        private string nextAttackName;
        private SpriteRenderer[] spriteRenderers;
        private AttackCommandHandler attackCommandHandler;

        protected override void OnInitialized()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            EventBus.Subscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
            EventBus.Subscribe<OnDamageCalculated>(OnDamageCalculated);
            EventBus.Subscribe<OnAttackCommandAllMatched>(OnAttackCommandAllMatched);

            spriteRenderer.material = new Material(spriteRenderer.material);
            string[] attackNames = new string[attackInfos.Length];
            for (int i = 0; i < attackInfos.Length; i++)
            {
                attackNames[i] = attackInfos[i].attackName;
            }
            attackCommandHandler = new AttackCommandHandler(attackNames, "ComboAttack04");
            attackCommandHandler.StartListening();
        }

        protected override void OnDisposed()
        {
            EventBus.Unsubscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
            EventBus.Unsubscribe<OnDamageCalculated>(OnDamageCalculated);
            EventBus.Unsubscribe<OnAttackCommandAllMatched>(OnAttackCommandAllMatched);
            attackCommandHandler.StopListening();
            attackCommandHandler = null;
        }

        private void OnAttackButtonPressed(OnAttackButtonPressed e)
        {
            if ((!IsPlaying("Idle") && GetNormalizedTime() < 0.9f) || isDead)
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
                if (!IsPlaying("Idle") && GetNormalizedTime() < 1f)
                {
                    CombatActor enemyActor = CombatActorContainer.GetActorByInstanceID(e.attackerActorInstanceID);
                    GameObject blockEffect = Instantiate(blockEffectPrefab, (enemyActor.transform.position + transform.position) / 2f + new Vector3(Random.Range(-0.3f, 0.3f), 1f + Random.Range(-0.2f, 0.2f), -5f), Quaternion.identity);
                    Destroy(blockEffect, 1f);
                    DamageNumberObject blockObject = Instantiate(blockObjectPrefab, blockEffect.transform.position + Vector3.up, Quaternion.identity);
                    blockObject.SetText("BLOCKED!");
                    blockObject.ShowAnimation(DamageNumberObject.AnimationType.UpFade);
                    Destroy(blockObject.gameObject, 1f);

                    EventBus.Publish(new OnAnyActorGotBlocked
                    {
                        blockCasterActorInstanceID = GetInstanceID(),
                        gotBlockedActorInstanceID = e.attackerActorInstanceID,
                        hitPosition = e.hitPosition
                    });

                    return;
                }

                StartCoroutine(IEApplyDamage(e));
            }
        }

        private void OnAttackCommandAllMatched(OnAttackCommandAllMatched e)
        {
            nextAttackName = e.returnAttackName;
        }

        private bool isShowingHitEffect = false;
        private IEnumerator IEApplyDamage(OnDamageCalculated e)
        {
            if (isShowingHitEffect || isDead)
                yield break;

            isShowingHitEffect = true;
            DOTween.To(() => GetColor(), SetColor, Color.red, 0.15f);

            EventBus.Publish(new OnAnyActorGotHit
            {
                attackerActorInstanceID = e.attackerActorInstanceID,
                targetActorInstanceID = e.targetActorInstanceID,
                damage = e.damage,
                hitPosition = e.hitPosition
            });

            Info.currentHP -= e.damage;
            DamageNumberObject damageNumberObject = Instantiate(damageNumberObjectPrefab, transform.position - Vector3.forward * 5f + Vector3.up, Quaternion.identity);
            damageNumberObject.SetDamage(e.damage);
            damageNumberObject.ShowAnimation(DamageNumberObject.AnimationType.Fall);

            if (Info.currentHP <= 0)
            {
                PlayAnimation("Die");
                isDead = true;
            }

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
                DOTween.To(GetBlackSpriteAlpha, SetBlackSpriteAlpha, 0f, 0.2f).OnComplete(() =>
                {
                    blackSpriteRenderer.gameObject.SetActive(false);
                });
            }
        }

        private float GetBlackSpriteAlpha()
        {
            return blackSpriteRenderer.color.a;
        }

        private void SetBlackSpriteAlpha(float alpha)
        {
            blackSpriteRenderer.color = new Color(0f, 0f, 0f, alpha);
        }

        private SpriteRenderer cloneChargeDoneSpriteRenderer;

        private float GetCloneChargeDoneSpriteRendererAlpha()
        {
            return cloneChargeDoneSpriteRenderer.color.a;
        }

        private void SetCloneChargeDoneSpriteRendererAlpha(float alpha)
        {
            cloneChargeDoneSpriteRenderer.color = new Color(1f, 1f, 1f, alpha);
        }

        private IEnumerator IECastAttack04()
        {
            chargeEffectRoot.SetActive(true);

            yield return new WaitForSeconds(1f);

            chargeDoneSpriteRenderer.sprite = spriteRenderer.sprite;
            cloneChargeDoneSpriteRenderer = Instantiate(chargeDoneSpriteRenderer);
            cloneChargeDoneSpriteRenderer.transform.position = chargeDoneSpriteRenderer.transform.position;
            cloneChargeDoneSpriteRenderer.transform.localScale = chargeDoneSpriteRenderer.transform.localScale;
            cloneChargeDoneSpriteRenderer.transform.rotation = chargeDoneSpriteRenderer.transform.rotation;
            cloneChargeDoneSpriteRenderer.gameObject.SetActive(true);
            chargeDoneEffect.SetActive(true);

            cloneChargeDoneSpriteRenderer.transform.DOScale(Vector3.one * 5f, 0.15f).SetEase(Ease.Linear);
            DOTween.To(GetCloneChargeDoneSpriteRendererAlpha, SetCloneChargeDoneSpriteRendererAlpha, 0f, 0.15f).SetEase(Ease.Linear);

            Material orginalMaterial = spriteRenderer.material;
            spriteRenderer.material = waveMaterial;

            yield return new WaitForSeconds(1.5f);

            spriteRenderer.material = orginalMaterial;

            Destroy(cloneChargeDoneSpriteRenderer.gameObject);
            chargeDoneEffect.SetActive(false);

            ResumeAnimation();

            while (GetNormalizedTime() < 0.33f)
            {
                yield return null;
            }

            isAttacked = true;

            attack04Effect.Play();
            chargeEffectRoot.SetActive(false);

            List<CombatActor> enemyActors = CombatActorContainer.GetAllActorInRange(ActorInfo.Camp.Enemy, transform.position, float.MaxValue - 1f);

            for (int enemyActorIndex = 0; enemyActorIndex < enemyActors.Count; enemyActorIndex++)
            {
                EnemyActor enemyActor = enemyActors[enemyActorIndex] as EnemyActor;

                EventBus.Publish(new OnAttackCasted
                {
                    attackerActorInstanceID = GetInstanceID(),
                    targetActorInstanceID = enemyActor.GetInstanceID(),
                    hitPosition = enemyActor.transform.position - Vector3.right * 0.2f
                });
            }

            Time.timeScale = 0.25f;

            yield return new WaitForSecondsRealtime(1f);

            Time.timeScale = 1f;
        }

        private void LateUpdate() // for detecting animation time
        {
            if (IsPlaying("Idle") || isDead)
                return;

            if (isAttacked)
            {
                if (!string.IsNullOrEmpty(nextAttackName) && GetNormalizedTime() >= 0.9f)
                {
                    blackSpriteRenderer.color = new Color(0f, 0f, 0f, 0f);
                    blackSpriteRenderer.gameObject.SetActive(true);
                    DOTween.To(GetBlackSpriteAlpha, SetBlackSpriteAlpha, 0.8f, 0.2f);
                    currentAttackName = nextAttackName;
                    nextAttackName = null;
                    isAttacked = false;
                    PlayAnimationAndStop(currentAttackName, 0.01f, 2f);

                    List<CombatActor> enemyActors = CombatActorContainer.GetAllActorInRange(ActorInfo.Camp.Enemy, transform.position, float.MaxValue - 1f);

                    for (int enemyActorIndex = 0; enemyActorIndex < enemyActors.Count; enemyActorIndex++)
                    {
                        EnemyActor enemyActor = enemyActors[enemyActorIndex] as EnemyActor;
                        enemyActor.Pause();
                    }

                    StartCoroutine(IECastAttack04());
                }

                return;
            }

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