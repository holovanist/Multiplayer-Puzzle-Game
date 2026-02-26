using Unity.Netcode;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Vector3 position;
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
        if(Reset)
        {
            ResetRpc();
        }
        if(CheckpointReached)
        {
            SetCheckpontRpc();
        }
    }
    [Rpc(SendTo.Everyone)]
    void ResetRpc()
    {
        Debug.Log("Reset");
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
    [Rpc(SendTo.ClientsAndHost)]
    void SetCheckpontRpc()
    {
        Debug.Log("set");
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        CheckpointReached = false;
    }
}
