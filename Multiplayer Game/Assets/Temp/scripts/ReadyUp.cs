using Unity.Netcode;
using UnityEngine;

public class ReadyUp : NetworkBehaviour
{
    [SerializeField]
    GameObject HostReadyUpIndicator;
    [SerializeField]
    GameObject ClientReadyUpIndicator;
    bool IsReady = false;
    public void Ready()
    {
        if(!IsReady && IsHost)
        {
            ReadyUpRpc(true, 1);
        }
        else if (IsReady && IsHost)
        {
            ReadyUpRpc(false, 1);
        }
        else if (!IsReady && !IsHost)
        {
            ReadyUpRpc(true, 2);
        }
        else if (IsReady && !IsHost)
        {
            ReadyUpRpc(false, 2);
        }
    }
    [Rpc(SendTo.Everyone)]
    private void ReadyUpRpc(bool ready, int Player)
    {
        IsReady = ready;
        if(Player == 1)
        {
            if(ready) HostReadyUpIndicator.SetActive(false);
            else if(!ready) HostReadyUpIndicator.SetActive(true);
        }
        if(Player == 2)
        {
            if(ready) ClientReadyUpIndicator.SetActive(false);
            else if(!ready) ClientReadyUpIndicator.SetActive(true);
        }
    }
}
