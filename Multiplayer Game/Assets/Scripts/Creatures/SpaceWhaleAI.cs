using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SpaceWhaleAI : MonoBehaviour
{
    [SerializeField] Transform SWBone1;
    [SerializeField] Transform SWBone1OffSet;
    [SerializeField] Transform PivotFollowObject;
    [SerializeField] Transform PivotOffset1;
    [SerializeField] Transform PivotOffset2;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float rotationSpeed = 0.01f;
    [SerializeField] bool useSetSpeed = true;
    public bool moveWhale = true;
    Transform target;
    bool state1 = true;
    private void Start()
    {
        target = PivotFollowObject;
    }
    void FixedUpdate()
    {
        if (moveWhale)
        {
            Figure8Patrol();
            MoveWhale(useSetSpeed);
        }
    }
    void Figure8Patrol()
    {
        //rotates pivots at set speed based off current state
        if (state1)
        {
            PivotOffset1.eulerAngles = new Vector3 (0f, PivotOffset1.eulerAngles.y + 0.25f, 0f);
        }
        else
        {
            PivotOffset2.eulerAngles = new Vector3(0f, PivotOffset2.eulerAngles.y - 0.25f, 0f);
        }
        if (PivotOffset1.eulerAngles.y >= -0.05 && PivotOffset1.eulerAngles.y <= 0.05 && state1 || PivotOffset2.eulerAngles.y >= -0.05 && PivotOffset2.eulerAngles.y <= 0.05! && !state1)
        {
            SwitchF8PatrolSide();
        }
    }
    void SwitchF8PatrolSide()
    {
        //switches the piviot point between the two sides when a full loop is completed
        if (state1)
        {
            state1 = false;
            PivotFollowObject.parent = PivotOffset2;
        } else
        {
            state1 = true;
            PivotFollowObject.parent = PivotOffset1;
        }
        //zeros out pivot rotations to counteract float drift
        PivotOffset1.eulerAngles = Vector3.zero;
        PivotOffset2.eulerAngles = Vector3.zero;
    }
    void MoveWhale(bool UseSetSpeed)
    {
        float appliedSpeed = moveSpeed;
        //rotates towards target with max rotation speed
        quaternion targetRotation = Quaternion.LookRotation(SWBone1.position - target.position);
        SWBone1.rotation = Quaternion.Lerp(SWBone1.rotation, targetRotation, rotationSpeed);

        //adds extra movespeed based off distance
        if (!UseSetSpeed)
        {
            Debug.Log("Ran!");
            appliedSpeed += ((Vector3.Distance(SWBone1.position, target.position) - 100) * 0.0001f);
            //clamps appliedSpeed at 1.5x the set move speed
            if (appliedSpeed > moveSpeed * 1.5) appliedSpeed = moveSpeed * 1.5f; Debug.Log("Clamped!");
        }


        //moves forward based off current rotation at set move speed
        Vector3 moveDir = SWBone1OffSet.position - SWBone1.position;
        SWBone1.position = SWBone1.position + (moveDir * appliedSpeed);

        //moves forward based off current rotation at varying move speed based off distance
        //SWBone1.position = SWBone1.position + (moveDir * (moveSpeed + (Vector3.Distance(SWBone1.position, target.position) * 0.0001f)));
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PivotOffset1.position, 200);
        Gizmos.DrawWireSphere(PivotOffset2.position, 200);
    }
}