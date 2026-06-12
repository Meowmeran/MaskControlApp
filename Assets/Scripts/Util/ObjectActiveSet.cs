using UnityEngine;

public class ObjectActiveSet : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectsToDeactivate;





    public void OnAction()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }
    }
}
