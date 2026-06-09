using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionButton : MonoBehaviour, INetworkButton
{
    [SerializeField] private Text _buttonText;
    [SerializeField] private Image _buttonImage;
    private UDPHandler _udp;
    [SerializeField] private LeanButton _buttonScript;
    private string _expressionName = "";
    private int _index = 0;
    private Color _color = Color.black;
    public void Start()
    {
        FindUDP();
        if (!CheckReferences())
        {
            Debug.LogError("[ExpressionButton] Missing references.");
        }
        _color = _buttonImage.color;
        AttachListener();
    }

    public void OnDestroy()
    {
        _buttonScript.OnClick.RemoveListener(OnClick);
    }


    public void OnClick()
    {
        _udp.SendSetExpression((byte)_index);
    }

    public void SetButton(string name, int index)
    {
        _expressionName = name;
        _index = index;
        _buttonText.text = name;
    }

    public Color SetColorFromPattern(int index)
    {
        index = index % 9;
        _color = index switch
        {
            0 => Color.black,
            1 => Color.white,
            2 => Color.red,
            3 => Color.green,
            4 => Color.blue,
            5 => Color.yellow,
            6 => Color.cyan,
            7 => Color.magenta,
            8 => Color.gray,
            _ => Color.black,
        };
        _buttonImage.color = _color;
        return WashoutColor(_color, 0.5f);

    }

    private Color WashoutColor(Color color, float intensity)
    {
        return new Color(color.r * intensity, color.g * intensity, color.b * intensity);
    }

    public void AttachListener()
    {
        _buttonScript.OnClick.AddListener(OnClick);
    }

    public void FindUDP()
    {
        _udp = FindAnyObjectByType<UDPHandler>();
    }

    public bool CheckReferences()
    {
        return _buttonText != null && _buttonImage != null && _buttonScript != null && _udp != null;
    }
}
