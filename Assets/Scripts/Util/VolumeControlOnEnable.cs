using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class VolumeControlOnEnable : MonoBehaviour
{
    private Volume volume;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float delay = 0f;
    
    void Awake()
    {
        volume = GetComponent<Volume>();
        volume.weight = 0f;    
    }
    private void OnEnable()
    {   
        volume.weight = 0f;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        yield return new WaitForSeconds(delay);
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            volume.weight = t / fadeTime;
            yield return null;
        }
        volume.weight = 1f;
    }

}
