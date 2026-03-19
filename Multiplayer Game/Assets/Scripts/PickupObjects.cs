using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    private Rigidbody hitRigidbody;
    private bool holdingObject = false;
    private bool spinMeRoundBabyRightRound = false;
    private InputAction _Interact;
    GameObject target;
    //RaycastHit hit;
    private void Start()
    {
        _Interact = GetComponent<PlayerInputHandler>().playerControls.FindAction("Interact");
    }
    private void Update()
    {
        if(hitRigidbody == null && hitObject != null)
            hitRigidbody = hitObject.GetComponent<Rigidbody>();
        if (_Interact.WasPressedThisFrame() && !holdingObject)
        {
            AttemptToPickupObject();
            
        } else if (_Interact.WasPressedThisFrame() && holdingObject )
        {
            if (IsLocalPlayer)
            {
                if (SOD != null)
                    SOD.IsHeld = false;
                NotHoldingRPC(hitObject.transform.position);
                target = hitRigidbody.transform.gameObject;
                var targetObject = target.GetComponent<NetworkObject>();
                targetObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                DropObjectRPC(targetObject);
            }
        }   
        if(SOD != null && SOD.IsInTunnel)
        {
            SOD.IsHeld = false;
            NonHoldingRPC();
            target = hitRigidbody.transform.gameObject;
            var targetObject = target.GetComponent<NetworkObject>();
            ObjectInTunnelRPC(targetObject);
        }
    }
    [Rpc(SendTo.Everyone)]
    private void NotHoldingRPC(Vector3 position)
    {
        if(hitObject != null)
        hitObject.transform.position = position;
        holdingObject = false;
        hitObject = null;
        hitRigidbody.useGravity = true;
    }    
    [Rpc(SendTo.Everyone)]
    private void NonHoldingRPC()
    {
        hitObject = null;
        holdingObject = false; 
    }
    void FixedUpdate()
    {
        if (holdingObject)
        {
            HoldingObject();
        }
    }
    private void HoldingObject()
    {
        if (hitObject.transform == null) return;
        Vector3 holdPosition;
        if (Physics.Raycast(transform.position, cameraTransform.forward, out RaycastHit hit, holdDistance , layerMask))
        {
            if(hit.transform.CompareTag("PlayerObject"))
            {
                holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
            }
            else
            {
                holdPosition = hit.point + new Vector3(-(cameraTransform.forward.x / 3), 1.6f, -(cameraTransform.forward.z / 3));
                hitObject.transform.position = holdPosition;
            }
        }   
        else
        {
         holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
        }
        if (Vector3.Distance(holdPosition, hitObject.transform.position) > MaxholdDistance)
        {
            if (IsLocalPlayer)
            {
                if (SOD != null)
                    SOD.IsHeld = false;
                NotHoldingRPC(hitObject.transform.position);
                target = hitRigidbody.transform.gameObject;
                var targetObject = target.GetComponent<NetworkObject>();
                targetObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                DropObjectRPC(targetObject);
            }
        }
        Vector3 velDirection = Vector3.Normalize(holdPosition - hitObject.transform.position);
        float distance = Vector3.Distance(holdPosition, hitObject.transform.position);
        if (hitObject.CompareTag("Physical Camera"))
        {
            hitObject.transform.position = holdPosition;
        }
        else if (distance > 0.05f)
        {
            hitRigidbody.linearVelocity = (multiplier * distance * velDirection);
        }
        else
        {
            hitRigidbody.linearVelocity = Vector3.zero;
            hitRigidbody.MovePosition(holdPosition);
        }
        if(SOD.ChangeRotation && SOD.IsHeld)
        {
            hitObject.transform.localRotation = SOD.RotationOffset;
        }
        else if (!spinMeRoundBabyRightRound)
        {
            hitObject.transform.rotation = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);
        }
        else
        {
            hitRigidbody.AddTorque(new Vector3(0, cameraTransform.rotation.eulerAngles.y, 0));
            hitObject.transform.rotation = Quaternion.Euler(-90, hitObject.transform.rotation.eulerAngles.y, 0);
        }
    }
    StoredObjectData SOD;
    private void AttemptToPickupObject()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupDistance, PickupLayerMask) && hitObject == null)
        {
            
            if (hit.collider.TryGetComponent(out StoredObjectData storedObjectData) && storedObjectData.isPickupable && !storedObjectData.IsInTunnel)
            {
                ParentObjectRPC();
                spinMeRoundBabyRightRound = storedObjectData.spinMeRoundBabyRightRound;
                target = hit.transform.gameObject;
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
    [Rpc(SendTo.Everyone)]
    private void PickupObjectRPC(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            StoredObjectData temp = targetObject.GetComponent<StoredObjectData>();
            SOD = temp;
            SOD.IsHeld = true;
            hitObject = temp.gameObject;
            hitRigidbody = targetObject.GetComponent<Rigidbody>();
            hitRigidbody.useGravity = false;
        }
        holdingObject = true;
    }
    [Rpc(SendTo.Server)]
    private void DropObjectRPC(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            targetObject.GetComponent<Rigidbody>().useGravity = true;
            targetObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            targetObject.transform.parent = null;
        }
    }    
    [Rpc(SendTo.Server)]
    private void ObjectInTunnelRPC(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            targetObject.transform.parent = null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PickupObject") && hitObject == collision.gameObject || collision.gameObject.CompareTag("Physical Camera") && hitObject == collision.gameObject)
        {
            NotHoldingRPC(hitObject.transform.position);
            if (SOD != null)
                SOD.IsHeld = false;
            target = hitRigidbody.transform.gameObject;
            var targetObject = target.GetComponent<NetworkObject>();
            targetObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            DropObjectRPC(targetObject);
        }
    }
}
