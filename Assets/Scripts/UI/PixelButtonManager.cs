using System.Collections.Generic;
using UnityEngine;

public class PixelButtonManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject leftEyePanel;
    [SerializeField] private GameObject rightEyePanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject horizontalGroupPrefab;
    [SerializeField] private GameObject buttonPrefab;
    [Header("References")]
    [SerializeField] private FlexibleColorPicker fcp;
    [SerializeField] private Color currentColor;
    [SerializeField] private UDPHandler _udp;
    private List<PixelButton> buttons = new List<PixelButton>();


    void Start()
    {
        if (_udp == null)
        {
            _udp = FindAnyObjectByType<UDPHandler>();
            if (_udp == null)
            {
                Debug.LogWarning("[PixelButtonManager] UDP not ready.");
            }
        }
        fcp.onColorChange.AddListener(SetColor);
        Generate();
    }

    void OnDestroy()
    {
        fcp.onColorChange.RemoveListener(SetColor);
    }

    public void SetColor(Color color)
    {
        currentColor = color;
    }

    public Color GetColor()
    {
        return currentColor;
    }
    public void Sync()
    {
        Color32[] Left = new Color32[16];
        Color32[] Right = new Color32[16];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                Left[i * 4 + j] = buttons[i * 4 + 3 - j].GetColor();
                Right[i * 4 + j] = buttons[i * 4 + 3 - j + 16].GetColor();
            }
        }
        _udp.SendSetFrame(Left, Right);
    }


    public void Clear()
    {
        foreach (PixelButton button in buttons)
        {
            button.Clear();
        }
        Sync();
    }

    private void Generate()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject horizontalGroup;
            if (i < 4)
            {
                horizontalGroup = Instantiate(horizontalGroupPrefab, leftEyePanel.transform);
            }
            else
            {
                horizontalGroup = Instantiate(horizontalGroupPrefab, rightEyePanel.transform);
            }
            for (int j = 0; j < 4; j++)
            {
                PixelButton button = Instantiate(buttonPrefab, horizontalGroup.transform).GetComponent<PixelButton>();
                buttons.Add(button);
                button.SetManager(this);
                button.SetIndex(i * 4 + j);
            }
        }
    }

}
