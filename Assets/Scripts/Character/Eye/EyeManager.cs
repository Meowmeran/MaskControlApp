using UnityEngine;

public class EyeManager : MonoBehaviour
{
    [SerializeField] private UDPReceiver _receiver;
    public EyePixelManager _left;
    public EyePixelManager _right;

    public float pixelSize = 0.01f;
    public float pixelSpacing = 0.01f;
    public const int rows = 4;
    public const int cols = 4;

    void Start()
    {
        SetSettings();
        _left.Generate();
        _right.Generate();

        _receiver.OnStateReceived += OnMaskStateReceived;
    }

    void OnDestroy()
    {
        _receiver.OnStateReceived -= OnMaskStateReceived;
    }

    void Update()
    {
        SetSettings();
    }

    [ContextMenu("Regenerate")]
    void Regenerate()
    {
        _left.Regenerate();
        _right.Regenerate();
    }

    void SetSettings()
    {
        _left.SetSettings(pixelSize, pixelSpacing, rows, cols);
        _right.SetSettings(pixelSize, pixelSpacing, rows, cols);
    }

    private void OnMaskStateReceived(MaskState state)
    {
        _left.Clear();
        _right.Clear();

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                Color32 left = state.GetPixel(leftEye: true, row, col);
                Color32 right = state.GetPixel(leftEye: false, row, col);

                if (!IsBlack(left)) _left.SetPixelColor(col, row, left);
                if (!IsBlack(right)) _right.SetPixelColor(col, row, right);
            }
        }
    }


    private static bool IsBlack(Color32 c)
        => c.r == 0 && c.g == 0 && c.b == 0;
}
