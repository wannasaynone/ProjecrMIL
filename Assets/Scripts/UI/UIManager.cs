using ProjectMIL.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIBase[] userInterfaces;

    private void Awake()
    {
        for (int i = 0; i < userInterfaces.Length; i++)
        {
            userInterfaces[i].Initial();
        }
    }
}
