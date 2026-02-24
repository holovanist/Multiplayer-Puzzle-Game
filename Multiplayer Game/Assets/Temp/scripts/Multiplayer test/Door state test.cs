using Unity.Netcode;
using UnityEngine;

public class Door : NetworkBehaviour
{
    public NetworkVariable<bool> State = new NetworkVariable<bool>();
    public bool test;

    private void Update()
    {
        if(test)
        {
            ToggleServerRpc();
            test = false;
        }
    }
    public override void OnNetworkSpawn()
    {
        State.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        State.OnValueChanged -= OnStateChanged;
    }

    public void OnStateChanged(bool previous, bool current)
    {
        // note: `State.Value` will be equal to `current` here
        if (State.Value)
        {
            Debug.Log("Open");
            // door is open:
            //  - rotate door transform
            //  - play animations, sound etc.
        }
        else
        {
            Debug.Log("Closed");
            // door is closed:
            //  - rotate door transform
            //  - play animations, sound etc.
        }
    }

    [Rpc(SendTo.Server)]
    public void ToggleServerRpc()
    {
        // this will cause a replication over the network
        // and ultimately invoke `OnValueChanged` on receivers
        State.Value = !State.Value;
    }
}
