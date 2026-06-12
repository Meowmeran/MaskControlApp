using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    [SerializeField] private CameraLerpBetweenPoints camSwitch;
    private int currentPage = 0;
    void Start()
    {
        ChangeTo(currentPage);
    }
    private void ChangeTo(int page)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == page);
        }
    }

[ContextMenu("MainPage")]
    public void MainPage()
    {
        ChangeTo(0);
        camSwitch.SwitchTo(0);
    }

[ContextMenu("EyeLookPage")]
    public void EyeLookPage()
    {
        ChangeTo(1);
        camSwitch.SwitchTo(1);
    }


}
