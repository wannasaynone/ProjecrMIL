using ProjectMIL.GameEvent;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class CombatButtonGroup : MonoBehaviour
    {
        public void Button_OnAttackButtonPressed(string attackName)
        {
            EventBus.Publish(new OnAttackButtonPressed { attackName = attackName });
        }
    }
}