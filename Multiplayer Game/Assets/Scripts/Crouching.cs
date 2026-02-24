using Unity.Netcode;
using UnityEngine;

public class Crouching : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    CapsuleCollider Collider;
    bool IsCrouching = false;
    bool stateChaged = false;
    private void Start()
    {
        Collider = GetComponentInChildren<CapsuleCollider>();
    }
    [Rpc(SendTo.Everyone)]
    public void CrouchRPC(bool Crouching, float ColliderHeight, Vector3 ColliderCenter, float CCHeight, Vector3 CCCenter)
    {
        stateChaged = true;
        IsCrouching = Crouching;
        Crouch(ColliderHeight, ColliderCenter, CCHeight, CCCenter);
        stateChaged = false;
    }
    void Crouch(float ColliderHeight, Vector3 ColliderCenter, float CCHeight, Vector3 CCCenter)
    {
        if (!stateChaged) return;  
        Collider.height = ColliderHeight;
        Collider.center = ColliderCenter;
        characterController.height = CCHeight;
        characterController.center = CCCenter;
    }
}
