using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class OnOffToggle : MonoBehaviour, INetworkButton
{
    private UDPHandler _udp;
    [SerializeField] private LeanButton _button;
    [SerializeField] private Text _text;
    private bool _isOn = false;
    public void AttachListener()
    {
        _button.OnClick.AddListener(OnClick);
    }

    public bool CheckReferences()
    {
        if (_udp == null || _button == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void FindUDP()
    {
        _udp = FindAnyObjectByType<UDPHandler>();
    }

    public void OnClick()
    {
        _udp.SendSetMode(SetButton() ? MaskMode.Active : MaskMode.Off);
    }

    public void OnDestroy()
    {
        _button.OnClick.RemoveListener(OnClick);
    }

    public void SetButton(string name, int index)
    {
        _text.text = name;
    }

    public bool SetButton()
    {
        _isOn = !_isOn;
        _text.text = _isOn ? "ON" : "OFF";
        _text.color = _isOn ? Color.green : Color.red;
        return _isOn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindUDP();
        if (!CheckReferences())
        {
            Debug.LogError("[ExpressionButton] Missing references.");
        }
        AttachListener();

    }

    void INetworkButton.Start()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
