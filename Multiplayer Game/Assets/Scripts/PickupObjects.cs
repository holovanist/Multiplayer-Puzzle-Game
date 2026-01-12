using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickupObjects : MonoBehaviour
{
    [SerializeField] float pickupDistance = 2;
    [SerializeField] float holdDistance = 2;
    public bool playerCanPickupObjects = true;
    [SerializeField] Transform cameraTransform;
    private GameObject hitObject;
    private Transform hitTransform;
    private Rigidbody hitRigidbody;
    private bool holdingObject = false;
    private bool spinMeRoundBabyRightRound = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !holdingObject)
        {
            AttemptToPickupObject();
            
        } else if (Input.GetKeyDown(KeyCode.E) && holdingObject)
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
        Vector3 velDirection = Vector3.Normalize(holdPosition - hitTransform.position);
        float distance = Vector3.Distance(holdPosition, hitTransform.position);
        if (distance > 0.05f)
        {
            hitRigidbody.velocity = (velDirection * 5 * distance);
        }
        else
        {
            hitRigidbody.velocity = Vector3.zero;
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
    private void AttemptToPickupObject()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupDistance))
        {
            
            if (hit.collider.TryGetComponent(out StoredObjectData storedObjectData) && hit.collider.TryGetComponent(out Rigidbody rigidbody) && storedObjectData.isPickupable)
            {
                hitObject = storedObjectData.gameObject;
                hitTransform = hitObject.transform;
                hitRigidbody = rigidbody;
                spinMeRoundBabyRightRound = storedObjectData.spinMeRoundBabyRightRound;
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
        holdingObject = false;
        hitRigidbody.useGravity = true;
        Debug.Log("dropped object");
    }
}
