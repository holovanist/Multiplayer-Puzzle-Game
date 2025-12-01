using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class RPC : NetworkBehaviour
{
    InputAction P;
    [Rpc(SendTo.Server)]
    public void PingRpc(int pingCount)
    {
        // Server -> Clients because PongRpc sends to NotServer
        // Note: This will send to all clients.
        // Sending to the specific client that requested the pong will be discussed in the next section.
        PongRpc(pingCount, "PONG!");
    }

    [Rpc(SendTo.NotServer)]
    void PongRpc(int pingCount, string message)
    {
        Debug.Log($"Received pong from server for ping {pingCount} and message {message}");
    }

    void Update()
    {
        if (IsOwner)
            P ??= InputSystem.actions.FindAction("Test");
        if (IsOwner && P.WasPressedThisFrame())
        {
            // Client -> Server because PingRpc sends to Server
            PingRpc(1);
        }
    }
}
