using UnityEngine;

public interface INetworkButton
{
    void Start();
    void OnClick();
    void SetButton(string name, int index);
    bool CheckReferences();
    void AttachListener();
    void FindUDP();
    void OnDestroy();
}