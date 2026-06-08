using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpressionTabManager : MonoBehaviour
{
    [SerializeField] private GameObject targetVerticalLayout;
    [SerializeField] private GameObject horizontalLayoutForExpressionsPrefab;
    private List<GameObject> horizontalLayouts = new List<GameObject>();
    [SerializeField] private GameObject expressionButtonPrefab;
    private List<ExpressionButton> expressionButtons = new List<ExpressionButton>();
    [SerializeField] private string expressionRaw = "";
    private List<string> strings = new List<string>();
    [SerializeField] private string[] stringsToSkip;
    private int expressionCount = 0;

    void Start()
    {
        ParseExpressions();
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        for (int i = 0; i < strings.Count; i++)
        {
            if (stringsToSkip.Contains(strings[i]) == false)
            {
                InstantiateButton(strings[i], i);
            }
        }
    }

    private void InstantiateButton(string expression, int i)
    {
        int buttonPerLine = 3;
        if  (expressionCount % buttonPerLine == 0)
        {
            var horizontalLayout = Instantiate(horizontalLayoutForExpressionsPrefab, targetVerticalLayout.transform);
            horizontalLayouts.Add(horizontalLayout);
        }

        var button = Instantiate(expressionButtonPrefab, horizontalLayouts[expressionCount / buttonPerLine].transform);
        button.GetComponent<ExpressionButton>().SetButton(expression, i);
        expressionButtons.Add(button.GetComponent<ExpressionButton>());
        expressionCount++;
    }

    [ContextMenu("ReassignButtonsToHorizontalLayouts")]
    private void ReassignButtonsToHorizontalLayouts()
    {
        for (int i = 0; i < expressionButtons.Count; i++)
        {
            expressionButtons[i].transform.parent = horizontalLayouts[i / 3].transform;
        }
    }

    private void ParseExpressions()
    {
        strings.Clear();

        strings.AddRange(
            expressionRaw
                .Split(new[] { ',', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
        );
    }
}
