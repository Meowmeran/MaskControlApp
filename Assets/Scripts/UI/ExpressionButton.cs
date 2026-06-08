using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionButton : MonoBehaviour
{
    [SerializeField] private Text _buttonText;
    [SerializeField] private Image _buttonImage;
    private UDPHandler _udp;
    private LeanButton _buttonScript;
    private string _expressionName = "";
    private int _index = 0;
    private Color _color = Color.black;
    void Start()
    {
        if  (_buttonText == null)
        {
            Debug.LogError("[ExpressionButton] No Text found.");
        }
        if  (_buttonImage == null)
        {
            Debug.LogError("[ExpressionButton] No Image found.");
        }
        _color = _buttonImage.color;
        _buttonScript = GetComponent<LeanButton>();
        if  (_buttonScript == null)
        {
            Debug.LogError("[ExpressionButton] No LeanButton found.");
        }
        _udp = FindAnyObjectByType<UDPHandler>();
        if  (_udp == null)
        {
            Debug.LogError("[ExpressionButton] No UDPHandler found.");
        }

        _buttonScript.OnClick.AddListener(OnClick);
    }

    void OnDestroy()
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

    public Color GetRandomColor()
    {
        return UnityEngine.Random.ColorHSV();
    }
}
