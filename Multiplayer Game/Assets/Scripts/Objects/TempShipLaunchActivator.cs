using Unity.Netcode;
using UnityEngine;

public class TempShipLaunchActivator : NetworkBehaviour
{
    [SerializeField] GameObject ShipObject;
    bool ran = false;
    [Rpc(SendTo.Everyone)]
    public void ActivateRPC()
    {
        Debug.Log("active");
        if (ran) return;
            ran = true;
            ShipObject.GetComponent<ShipLauncher>().StartShipLaunch();
    }
}
