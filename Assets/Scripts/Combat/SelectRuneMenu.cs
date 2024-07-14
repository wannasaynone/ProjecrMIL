using System;
using UnityEngine;

namespace ProjectMIL.Combat
{
    public class SelectRuneMenu : MonoBehaviour
    {
        private Action<int> onSelected;

        public void StartSelect(Action<int> onSelected)
        {
            this.onSelected = onSelected;
            gameObject.SetActive(true);
        }

        public void Button_Select(int index)
        {
            onSelected?.Invoke(index);
            gameObject.SetActive(false);
        }
    }
}