using System;
using System.Collections;
using System.Collections.Generic;
using ProjectMIL.GameEvent;
using UnityEngine;
using DG.Tweening;

namespace ProjectMIL.Combat
{
    public class EnemyActor : CombatActor
    {
        [SerializeField] private GameObject hitEffectPrefab;

        private SpriteRenderer[] spriteRenderers;

        protected override void OnInitialized()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            EventBus.Subscribe<OnAttackCasted>(OnAttackerAttacked);
            EventBus.Subscribe<OnDamageCalculated>(OnDamageCalculated);
        }

        private void OnAttackerAttacked(OnAttackCasted e)
        {
            if (e.targetActorInstanceID == GetInstanceID())
            {

            }
        }

        private void OnDamageCalculated(OnDamageCalculated e)
        {
            if (e.targetActorInstanceID == GetInstanceID())
            {
                StartCoroutine(IEGotHit(e));
            }
        }

        private void SetColor(Color color)
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.name == "Shadow")
                    continue;

                spriteRenderer.color = color;
            }
        }

        private Color GetColor()
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.name == "Shadow")
                    continue;

                return spriteRenderer.color;
            }

            return Color.white;
        }

        private bool isShowingHitEffect = false;
        private IEnumerator IEGotHit(OnDamageCalculated e)
        {
            if (isShowingHitEffect)
                yield break;

            isShowingHitEffect = true;

            DOTween.To(() => GetColor(), SetColor, Color.red, 0.15f);
            PlayAnimation("3_Debuff_Stun", 1.5f);
            transform.DOMove(transform.position + Vector3.right * UnityEngine.Random.Range(1f, 3f), 0.15f);

            GameObject cloneHitEffect = Instantiate(hitEffectPrefab, e.hitPosition, Quaternion.identity);
            Destroy(cloneHitEffect, 1f);

            EventBus.Publish(new OnGotHit
            {
                attackerActorInstanceID = e.attackerActorInstanceID,
                targetActorInstanceID = e.targetActorInstanceID,
                hitPosition = e.hitPosition,
                damage = e.damage
            });

            Debug.Log("Enemy got " + e.damage + " damage");

            yield return new WaitForSeconds(0.15f);

            DOTween.To(() => GetColor(), SetColor, Color.white, 0.15f);

            yield return new WaitForSeconds(0.15f);

            PlayAnimation("0_idle", 1f);
            isShowingHitEffect = false;
        }
    }
}