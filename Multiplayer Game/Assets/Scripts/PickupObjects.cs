using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObjects : NetworkBehaviour
{
    [SerializeField] float pickupDistance = 2;
    [SerializeField] float holdDistance = 2;
    public bool playerCanPickupObjects = true;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform Parent;
    [SerializeField] float multiplier;
    [SerializeField] float MaxholdDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask PickupLayerMask;
    private GameObject hitObject;
    private Transform hitTransform;
    private Rigidbody hitRigidbody;
    private bool holdingObject = false;
    private bool spinMeRoundBabyRightRound = false;
    private InputAction _Interact;
    bool InteractPressed;
    RaycastHit hit;
    private void Start()
    {
        _Interact = GetComponent<PlayerInputHandler>().playerControls.FindAction("Interact");
    }
    private void Update()
    {
        if (_Interact.WasPressedThisFrame() && !holdingObject)
        {
            AttemptToPickupObject();
            
        } else if (_Interact.WasPressedThisFrame() && holdingObject )
        {
            PressedInteractRPC();
        }   
        if(InteractPressed)
        {
            DropObject();
            
        }
    }
    void FixedUpdate()
    {
        if (holdingObject)
        {
            HoldingObject();
        }
    }
    [Rpc(SendTo.Server)]
    private void PressedInteractRPC()
    {
        InteractPressed = true;
    }
    private void HoldingObject()
    {
        Vector3 holdPosition;
        if (Physics.Raycast(transform.position, cameraTransform.forward, out hit, holdDistance , layerMask))
        {
            holdPosition = hit.point + new Vector3(0, .6f,0);
            hitTransform.position = holdPosition;
        }
        else
        {
         holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;

        }
        if (Parent != null)
        {
            if (IsHost)
                hitTransform.parent = Parent.transform;                
            else if (IsClient)
                ParentObjectRPC();
        }
        if (Vector3.Distance(holdPosition, hitTransform.position) > MaxholdDistance)
        {
            DropObject();
        }
        Vector3 velDirection = Vector3.Normalize(holdPosition - hitTransform.position);
        float distance = Vector3.Distance(holdPosition, hitTransform.position);
        if (distance > 0.05f)
        {
            hitRigidbody.linearVelocity = (multiplier * distance * velDirection);
        }
        else
        {
            hitRigidbody.linearVelocity = Vector3.zero;
            hitRigidbody.MovePosition(holdPosition);
        }
        if (!spinMeRoundBabyRightRound)
        {
            hitTransform.rotation = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);
        }
        else
        {
            hitRigidbody.AddTorque(new Vector3(0, cameraTransform.rotation.eulerAngles.y, 0));
            hitTransform.rotation = Quaternion.Euler(-90, hitTransform.rotation.eulerAngles.y, 0);
        }
    }
    StoredObjectData SOD;
    private void AttemptToPickupObject()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupDistance, PickupLayerMask))
        {
            
            if (hit.collider.TryGetComponent(out StoredObjectData storedObjectData) && hit.collider.TryGetComponent(out Rigidbody rigidbody) && storedObjectData.isPickupable && !storedObjectData.IsInTunnel)
            {
                if (IsHost)
                    hit.transform.parent = Parent;
                else if (IsClient)
                    ParentObjectRPC();
                spinMeRoundBabyRightRound = storedObjectData.spinMeRoundBabyRightRound;
                storedObjectData.IsHeld = true;
                GameObject target = hit.transform.gameObject;
                var targetObject = target.GetComponent<NetworkObject>();
                PickupObjectRPC(targetObject);
            }
        }
    }
    [Rpc(SendTo.Server)]
    private void ParentObjectRPC()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupDistance, PickupLayerMask))
            hit.transform.parent = Parent;
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void PickupObjectRPC(NetworkObjectReference target)
    {
        holdingObject = true;
        if (target.TryGet(out NetworkObject targetObject))
        {
            StoredObjectData temp = targetObject.GetComponent<StoredObjectData>();
            SOD = temp;
            hitObject = temp.gameObject;
            hitTransform = hitObject.transform;
            hitRigidbody = targetObject.GetComponent<Rigidbody>();
            hitRigidbody.useGravity = false;
        }
    }
    [Rpc(SendTo.Server)]
    private void UnparentObjectRPC(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            targetObject.GetComponent<Rigidbody>().useGravity = true;
            targetObject.transform.parent = null;
            InteractPressed = false;
        }
    }
    private void DropObject()
    {
        if (SOD !=null)
        SOD.IsHeld = false;
        holdingObject = false;
        if (IsHost)
        {
            hitRigidbody.useGravity = true;
            hitObject.transform.parent = null;
            InteractPressed = false;
        }
        else if (IsClient)
        {
            hitRigidbody.useGravity = true;
            GameObject target = hitRigidbody.transform.gameObject;
            var targetObject = target.GetComponent<NetworkObject>();
            UnparentObjectRPC(targetObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && holdingObject)
        {
            DropObject();
        }
    }
}
