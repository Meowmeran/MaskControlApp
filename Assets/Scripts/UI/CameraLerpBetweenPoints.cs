using Unity.Cinemachine;
using UnityEngine;

public class CameraLerpBetweenPoints : MonoBehaviour
{
    [SerializeField] private CinemachineCamera[] cams;

    private void Start()
    {
        SwitchTo(0);
    }

    public void SwitchTo(int index)
    {
        for (int i = 0; i < cams.Length; i++)
        {
            if (i == index)
            {
                cams[i].Priority = 10;
                cams[i].gameObject.SetActive(true);
            }
            else
            {
                cams[i].Priority = 0;
                cams[i].gameObject.SetActive(false);
            }
        }
    }
}
