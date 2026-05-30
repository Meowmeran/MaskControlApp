using UnityEngine;

[RequireComponent(typeof(UIPageManager))]
public class UIHandler : MonoBehaviour
{
    private UIPageManager uiPageManager;

    void Start()
    {
        uiPageManager = GetComponent<UIPageManager>();
    }

    public void NextPage() => uiPageManager.NextPage();
    public void PreviousPage() => uiPageManager.PreviousPage();
}
