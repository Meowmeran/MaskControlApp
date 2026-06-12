using System;
using UnityEngine;
using UnityEngine.UI;

public class PixelButton : MonoBehaviour
{
    public Color color = Color.gray;
    private Image _buttonImage;
    private UDPHandler _udp;
    private PixelButtonManager _manager;
    [SerializeField] private int index = 0;


    void Start()
    {
        _udp = FindAnyObjectByType<UDPHandler>();
        if (_udp == null)
        {
            Debug.LogWarning("[PixelButton] UDP not ready.");
        }
        _buttonImage = GetComponent<Image>();
        if (_buttonImage == null)
        {
            Debug.LogWarning("[PixelButton] Image not found.");
        }
        SetColor(color);
    }
    private void SetColor(Color color)
    {
        this.color = color;
        _buttonImage.color = color;
    }

    public void OnAction()
    {
        SetColor(_manager.GetColor());
        Send();
    }

    public void SetManager(PixelButtonManager manager)
    {
        _manager = manager;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void Clear()
    {
        SetColor(Color.black);
    }

    private void Send()
    {
        int localIndex = index % 16; // index within the eye (0-15)
        byte eye = (byte)(index < 16 ? 1 : 2); // 1 = left, 2 = right
        byte row = (byte)(localIndex / 4);
        byte col = (byte)(3 - localIndex % 4);
        _udp.SendSetColor(eye, row, col, color);
    }

    public void Sync()
    {
        Send();
    }

    internal Color32 GetColor()
    {
        return color;
    }
}
