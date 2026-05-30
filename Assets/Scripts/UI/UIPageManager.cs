using UnityEngine;

public class UIPageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private int activePage = -1;

    void Start()
    {
        ShowPage(0);
    }
    public int ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
        activePage = index;
        return activePage;
    }

    public void NextPage()
    {
        ShowPage((activePage + 1) % pages.Length);
    }
    public void PreviousPage()
    {
        ShowPage((activePage - 1 + pages.Length) % pages.Length);
    }
}
