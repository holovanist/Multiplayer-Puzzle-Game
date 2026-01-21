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
                hitRigidbody.useGravity = true;
                target = hitRigidbody.transform.gameObject;
                var targetObject = target.GetComponent<NetworkObject>();
                DropObjectRPC(targetObject);
            }
        }   
        if(SOD != null && SOD.IsInTunnel)
        {
            hitRigidbody.useGravity = true;
            SOD.IsHeld = false;
            NotHoldingRPC();
            target = hitRigidbody.transform.gameObject;
            var targetObject = target.GetComponent<NetworkObject>();
            DropObjectRPC(targetObject);
        }
    }
    [Rpc(SendTo.Everyone)]
    private void NotHoldingRPC()
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
        if(SOD.ChangeRotation)
        {
            //hitObject.transform.localRotation = SOD.RotationOffset;
        }
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
                ParentObjectRPC();
        }
        if (Vector3.Distance(holdPosition, hitTransform.position) > MaxholdDistance)
        {
            if (IsLocalPlayer && !IsOwnedByServer)
            {
                hitRigidbody.useGravity = true;
                target = hitRigidbody.transform.gameObject;
                var targetObject = target.GetComponent<NetworkObject>();
                DropObjectRPC(targetObject);
            }
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
                {
                    ParentObjectRPC();
                }
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
        hitObject = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && holdingObject)
        {
            GameObject target = hitRigidbody.transform.gameObject;
            var targetObject = target.GetComponent<NetworkObject>();
            DropObjectRPC(targetObject);
        }
    }
}
