using UnityEngine;

public class PlacePointAtScreenCorners : MonoBehaviour
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] bool place = false;

    private void Update()
    {
        Place();
    }

    private void Place()
    {
        if (!place) return;
        point1.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        point2.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
    }

}
