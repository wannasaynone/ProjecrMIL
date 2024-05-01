using ProjectMIL.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIBase[] userInterfaces;

    public void Initail()
    {
        for (int i = 0; i < userInterfaces.Length; i++)
        {
            userInterfaces[i].Initial();
        }
    }
}
