using UnityEngine;

public class Expressions : MonoBehaviour
{
    static public int NORMAL_EXPRESSION_END = 0;
    static public int SIZE = 0;
    static public int GetRandomExpression()
    {
        return Random.Range(0, SIZE);
    }

    static public int GetRandomExpression(int start, int end)
    {
        return Random.Range(start, end);
    }
}