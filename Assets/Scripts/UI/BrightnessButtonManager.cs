using System.Collections.Generic;
using UnityEngine;

public class BrightnessButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject brightnessButtonPrefab;
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private int[] brightnessSteps = { 1, 5, 10, 20, 40, 80, 160, 255 };


    void Start()
    {
        CleanupTargetPanelChildren();
        GenerateButtons();
    }


    private void GenerateButtons()
    {
        for (int i = 0; i < brightnessSteps.Length; i++)
        {
            if (brightnessSteps[i] < 0 || brightnessSteps[i] > 255) continue;
            Instantiate(brightnessButtonPrefab, targetPanel.transform).GetComponent<BrightnessButton>().SetButton(((int)Mathf.Ceil(100f * brightnessSteps[i] / 255F)).ToString() + "%", brightnessSteps[i]);
        }

    }
    private bool CleanupTargetPanelChildren()
    {
        if (targetPanel == null) return false;
        if (targetPanel.transform.childCount > 0)
        {
            for (int i = 0; i < targetPanel.transform.childCount; i++)
            {
                Destroy(targetPanel.transform.GetChild(i).gameObject);
            }
            return true;
        }
        return false;
    }
}
