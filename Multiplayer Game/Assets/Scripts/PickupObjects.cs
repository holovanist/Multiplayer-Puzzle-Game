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
    GameObject target;
    RaycastHit hit;
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
                NotHoldingRPC();
                target = hitRigidbody.transform.gameObject;
                var targetObject = target.GetComponent<NetworkObject>();
                hitObject = null;
                hitTransform = null; 
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
            hitObject = null;
            hitTransform = null;
        }
    }
    [Rpc(SendTo.Everyone)]
    private void NotHoldingRPC()
    {
        holdingObject = false; 
        hitRigidbody.useGravity = true;
    }    
    [Rpc(SendTo.Everyone)]
    private void NonHoldingRPC()
    {
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
        Vector3 holdPosition;
        if (Physics.Raycast(transform.position, cameraTransform.forward, out hit, holdDistance , layerMask))
        {
            holdPosition = hit.point + new Vector3(-(cameraTransform.forward.x / 3), .6f, -(cameraTransform.forward.z / 3));
            hitTransform.position = holdPosition;
        }   
        else
        {
         holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;

        }
/*        if (Parent != null && hitObject.transform.parent == null)
        {               
                //ParentObjectRPC();
        }*/
        if (Vector3.Distance(holdPosition, hitTransform.position) > MaxholdDistance)
        {
            if (IsLocalPlayer)
            {
                if (SOD != null)
                    SOD.IsHeld = false;
                NotHoldingRPC();
                target = hitRigidbody.transform.gameObject;
                hitObject = null;
                hitTransform = null;
                var targetObject = target.GetComponent<NetworkObject>(); 
                DropObjectRPC(targetObject);
            }
        }
        Vector3 velDirection = Vector3.Normalize(holdPosition - hitTransform.position);
        float distance = Vector3.Distance(holdPosition, hitTransform.position);
        if (hitObject.CompareTag("Physical Camera"))
        {
            hitTransform.position = holdPosition;
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
            hitTransform = hitObject.transform;
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
            if (SOD != null)
                SOD.IsHeld = false;
            NotHoldingRPC();
            target = hitRigidbody.transform.gameObject;
            var targetObject = target.GetComponent<NetworkObject>();
            hitObject = null;
            hitTransform = null;
            DropObjectRPC(targetObject);
        }
    }
}
