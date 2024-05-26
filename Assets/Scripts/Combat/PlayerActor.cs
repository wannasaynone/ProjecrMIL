using System.Collections;
using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class PlayerActor : CombatActor
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject speedLineEffectRoot;

        public override void Initialize()
        {
            EventBus.Subscribe<OnAttackButtonPressed>(OnAttackButtonPressed);
            spriteRenderer.material = new Material(spriteRenderer.material);

        }

        private void OnAttackButtonPressed(OnAttackButtonPressed e)
        {
            if (!IsPlaying("Idle") && GetNormalizedTime() < 0.9f)
                return;

            CombatUnit enemyUnit = CombatUnitContainer.GetCloestUnitByCamp(CombatUnit.Camp.Enemy, transform.position);
            if (enemyUnit == null)
            {
                PlayAnimation(e.attackName, 2f);
            }
            else
            {
                PlayAnimationAndStop(e.attackName, 0.01f, 2f);
                StartCoroutine(IEDashToEnemy(enemyUnit));
            }
        }

        private IEnumerator IEDashToEnemy(CombatUnit enemyUnit)
        {
            float motionBlur = 0f;
            while (Vector3.Distance(transform.position, enemyUnit.Actor.transform.position) > 2f)
            {
                float moveSpeed = 25f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, enemyUnit.Actor.transform.position, moveSpeed);
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
        }
    }
}