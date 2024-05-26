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
        private SpriteRenderer[] spriteRenderers;

        protected override void OnInitialized()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            EventBus.Subscribe<OnStartToHit>(OnStartToHit);
        }

        private void OnStartToHit(OnStartToHit e)
        {
            if (e.targetActorInstanceID == GetInstanceID())
            {
                StartCoroutine(IEStartToHit());
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
        private IEnumerator IEStartToHit()
        {
            if (isShowingHitEffect)
                yield break;

            isShowingHitEffect = true;

            DOTween.To(() => GetColor(), SetColor, Color.red, 0.15f);
            PlayAnimation("3_Debuff_Stun", 1.5f);

            yield return new WaitForSeconds(0.15f);

            DOTween.To(() => GetColor(), SetColor, Color.white, 0.15f);

            yield return new WaitForSeconds(0.15f);

            PlayAnimation("0_idle", 1f);
            isShowingHitEffect = false;
        }
    }
}