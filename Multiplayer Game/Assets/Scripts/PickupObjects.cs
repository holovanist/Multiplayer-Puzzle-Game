using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObjects : MonoBehaviour
{
    [SerializeField] float pickupDistance = 2;
    [SerializeField] float holdDistance = 2;
    public bool playerCanPickupObjects = true;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform Parent;
    private GameObject hitObject;
    private Transform hitTransform;
    private Rigidbody hitRigidbody;
    private bool holdingObject = false;
    private bool spinMeRoundBabyRightRound = false;
    private InputAction _Interact;
    private void Start()
    {
        _Interact = GetComponent<PlayerInputHandler>().playerControls.FindAction("Interact");
    }
    void Update()
    {
        if (_Interact.WasPressedThisFrame() && !holdingObject)
        {
            AttemptToPickupObject();
            
        } else if (_Interact.WasPressedThisFrame() && holdingObject || holdingObject && SOD.IsInTunnel)
        {
            DropObject();
        }
        if (holdingObject)
        {
            HoldingObject();
        }
    }
    private void HoldingObject()
    {
        Vector3 holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
        if(Parent != null)
        Parent.transform.position = holdPosition;
        Vector3 velDirection = Vector3.Normalize(holdPosition - hitTransform.position);
        float distance = Vector3.Distance(holdPosition, hitTransform.position);
        if (distance > 0.05f)
        {
            hitRigidbody.linearVelocity = (5 * distance * velDirection);
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
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupDistance))
        {
            
            if (hit.collider.TryGetComponent(out StoredObjectData storedObjectData) && hit.collider.TryGetComponent(out Rigidbody rigidbody) && storedObjectData.isPickupable && !storedObjectData.IsInTunnel)
            {
                hit.transform.parent = Parent;
                SOD = storedObjectData;
                hitObject = storedObjectData.gameObject;
                hitTransform = hitObject.transform;
                hitRigidbody = rigidbody;
                spinMeRoundBabyRightRound = storedObjectData.spinMeRoundBabyRightRound;
                storedObjectData.IsHeld = true;
                PickupObject();
            }
        }
    }
    private void PickupObject()
    {
        holdingObject = true;
        hitRigidbody.useGravity = false;
        Debug.Log("pickedup object");
    }
    private void DropObject()
    {
        if (SOD !=null)
        SOD.IsHeld = false;
        holdingObject = false;
        hitRigidbody.useGravity = true;
        hitObject.transform.parent = null;
        Debug.Log("dropped object");
    }
}
