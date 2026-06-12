using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessButton : MonoBehaviour, INetworkButton
{

    [SerializeField] private Text _buttonText;
    [SerializeField] private Image _buttonImage;
    private UDPHandler _udp;
    [SerializeField] private LeanButton _buttonScript;
    [SerializeField] private int _step = 0;


    public void Start()
    {
        FindUDP();
        if (!CheckReferences())
        {
            Debug.LogError("[ExpressionButton] Missing references.");
        }
        AttachListener();
    }
    public void AttachListener()
    {
        _buttonScript.OnClick.AddListener(OnClick);
    }

    public bool CheckReferences()
    {
        return _buttonText != null && _buttonImage != null && _buttonScript != null && _udp != null;
    }

    public void FindUDP()
    {
        _udp = FindAnyObjectByType<UDPHandler>();
    }

    public void OnClick()
    {
        _udp.SendSetBrightness((byte)_step);
    }

    public void OnDestroy()
    {
        _buttonScript.OnClick.RemoveListener(OnClick);
    }

    public void SetButton(string name, int index)
    {
        _buttonText.text = name;
        _step = index;
    }
    public void SetButton(int index)
    {
        _buttonText.text = index.ToString();
        _step = index;
    }
}
