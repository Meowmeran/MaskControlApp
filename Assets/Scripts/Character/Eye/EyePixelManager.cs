using System.Collections.Generic;
using UnityEngine;

public class EyePixelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pixelPrefab;
    private EyePixel[] pixels;
    [Header("Pixel Options")]
    private float pixelSize = 0.1f;
    private float pixelSpacing = 0.1f;
    int rows = 4;
    int cols = 4;

    private void Start()
    {

    }

    public void SetSettings(float pixelSize, float pixelSpacing, int rows, int cols)
    {
        this.pixelSize = pixelSize;
        this.pixelSpacing = pixelSpacing;
        this.rows = rows;
        this.cols = cols;
    }

    public void Generate()
    {
        if (pixels == null)
        {
            pixels = new EyePixel[rows * cols];
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject pixel = Instantiate(pixelPrefab, transform, false);
                pixel.transform.localPosition = new Vector3(j * pixelSpacing, -i * pixelSpacing, 0.0f);
                pixel.transform.localScale = new Vector3(pixelSize, pixelSize, pixelSize);
                pixels[i * cols + j] = pixel.GetComponent<EyePixel>();
            }
        }
    }

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
        if (pixels == null) return;
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == null)
            {
                Debug.LogError("Pixel " + i + " is null");
                continue;
            }
            Destroy(pixels[i].gameObject);
        }
        pixels = null;
        Generate();
    }

    private EyePixel GetPixel(int x, int y)
    {
        return pixels[y * cols + x];
    }

    [ContextMenu("Test")]
    public void Test()
    {
        Debug.Log(GetPixel(0, 0).gameObject.name);
    }

    public void SetPixelColor(int x, int y, Color color)
    {
        GetPixel(x, y).Activate();
        GetPixel(x, y).SetColor(color);
    }

    public void Clear()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                SetPixelColor(i, j, Color.black);
                GetPixel(i,j).Deactivate();
            }
        }
    }

    public void SetPixelIntensity(int x, int y, float intensity)
    {
        GetPixel(x, y).SetIntensity(intensity);
    }
}
