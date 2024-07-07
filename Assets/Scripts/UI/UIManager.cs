using ProjectMIL.GameEvent;
using ProjectMIL.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIBase[] userInterfaces;
    [SerializeField] private RectTransform mainUIGridRoot;

    public void Initialize(ProjectMIL.Utlity.ContextHandler contextHandler)
    {
        EventBus.Subscribe<OnBottomBarButtonPressed>(OnBottomBarButtonPressed);
        for (int i = 0; i < userInterfaces.Length; i++)
        {
            userInterfaces[i].Initialize(contextHandler);
        }
        UserInterfaceContextSetter[] userInterfaceContextSetters = FindObjectsOfType<UserInterfaceContextSetter>(true);
        foreach (UserInterfaceContextSetter userInterfaceContextSetter in userInterfaceContextSetters)
        {
            userInterfaceContextSetter.SetUp(contextHandler);
        }
    }

    private void OnBottomBarButtonPressed(OnBottomBarButtonPressed e)
    {
        DOTween.To(() => GetMainUIGridRootX(), SetMainUIGridRootX, 0f - 1080f * e.buttonIndex, 0.15f).SetEase(Ease.Linear);
    }

    private float GetMainUIGridRootX()
    {
        return mainUIGridRoot.anchoredPosition.x;
    }

    private void SetMainUIGridRootX(float x)
    {
        mainUIGridRoot.anchoredPosition = new Vector3(x, 0f, 0f);
    }
}
