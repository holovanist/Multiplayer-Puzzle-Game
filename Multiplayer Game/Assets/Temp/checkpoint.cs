using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    public Vector3 position {  get; private set; }
    Quaternion rotation;
    Vector3 scale;
    public bool Reset { get; set; }
    public bool CheckpointReached { get; set; }

    private void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        CheckpointReached = false;
    }
    private void Update()
    {
        if (Reset)
        {
            ResetRpc();
        }
        if (CheckpointReached)
        {
            SetCheckpontRpc();
        }
    }
    [Rpc(SendTo.Server)]
    void ResetRpc()
    {
        Debug.Log("Reset");
        if (TryGetComponent<CharacterController>(out var a))
        {
            a.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = scale;
            a.enabled = true;
            ResetFixRpc();
        }
        else
        {
            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = scale; ResetFixRpc();
        }
        Reset = false;
    }
    [Rpc(SendTo.ClientsAndHost)]
    void SetCheckpontRpc()
    {
        Debug.Log("set");
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        CheckpointReached = false;
    }
    [Rpc(SendTo.NotServer)]
    void ResetFixRpc()
    {
        Debug.Log("Reset2");
        if (TryGetComponent<CharacterController>(out var a))
        {
            a.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = scale;
            a.enabled = true;
        }
        else
        {
            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = scale;
        }
        Reset = false;
    }
}
