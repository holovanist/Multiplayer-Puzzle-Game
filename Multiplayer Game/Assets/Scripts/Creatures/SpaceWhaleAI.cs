using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SpaceWhaleAI : MonoBehaviour
{
    [SerializeField] Transform swBone1;
    [SerializeField] Transform swBone1Offset;
    [SerializeField] Transform pivotOffset1;
    [SerializeField] Transform pivotOffset2;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float maxExtraMoveSpeedMultiplier = 1.1f;
    [SerializeField] float rotationSpeed = 0.01f;
    [SerializeField] bool useSetSpeed = true;
    static Transform pivotFollowObject;
    static bool FollowingPlayer = false;
    public bool moveWhale = true;
    static Transform _target;
    bool state1 = true;
    private void Start()
    {
        GetPatrolFollowObject();
        ResumeFigure8Patrol();
    }
    void FixedUpdate()
    {
        if (moveWhale)
        {
            Figure8Patrol();
            if (FollowingPlayer)
            {
                MoveWhale(true);
            }
            else
            {
                MoveWhale(useSetSpeed);
            }
        }
    }
    public static void TargetObject(Transform target, bool isPlayer)
    {
        _target = target;
        FollowingPlayer = isPlayer;
    }
    void GetPatrolFollowObject()
    {
        pivotFollowObject = GameObject.FindGameObjectWithTag("swPivotFollowObject").transform;
    }
    public static void ResumeFigure8Patrol()
    {
        _target = pivotFollowObject;
    }
    void Figure8Patrol()
    {
        //rotates pivots at set speed based off current state
        if (state1)
        {
            pivotOffset1.eulerAngles = new Vector3 (0f, pivotOffset1.eulerAngles.y + 0.10f, 0f);
        }
        else
        {
            pivotOffset2.eulerAngles = new Vector3(0f, pivotOffset2.eulerAngles.y - 0.10f, 0f);
        }
        //checks if a state switch is needed
        if (pivotOffset1.eulerAngles.y >= -0.05 && pivotOffset1.eulerAngles.y <= 0.05 && state1 || pivotOffset2.eulerAngles.y >= -0.05 && pivotOffset2.eulerAngles.y <= 0.05! && !state1)
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
            pivotFollowObject.parent = pivotOffset2;
        } else
        {
            state1 = true;
            pivotFollowObject.parent = pivotOffset1;
        }
        //zeros out pivot rotations to counteract float drift
        pivotOffset1.eulerAngles = Vector3.zero;
        pivotOffset2.eulerAngles = Vector3.zero;
    }
    void MoveWhale(bool UseSetSpeed)
    {
        //by defalut the appliedSpeed is just movespeed
        float appliedSpeed = moveSpeed;

        //rotates towards target with max rotation speed
        quaternion targetRotation = Quaternion.LookRotation(swBone1.position - _target.position);
        swBone1.rotation = Quaternion.Lerp(swBone1.rotation, targetRotation, rotationSpeed);

        //adds extra movespeed based off distance if enabled
        if (!UseSetSpeed)
        {
            float dist = Vector3.Distance(swBone1.position, _target.position);
            //adds small amout of the distance between self and target to applied speed to increse speed with far targets
            //The -100 is so that if the distance is low enough it will reduce the speed instead of adding to it
            appliedSpeed += ((dist - 100) * 0.001f);
            //clamps appliedSpeed
            if (appliedSpeed > moveSpeed * maxExtraMoveSpeedMultiplier)
            {
                appliedSpeed = moveSpeed * maxExtraMoveSpeedMultiplier; 
            }
        }

        //movement direction
        Vector3 moveDir = swBone1Offset.position - swBone1.position;

        //moves forward based off current rotation at appliedSpeed
        swBone1.position = swBone1.position + (moveDir * appliedSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pivotOffset1.position, 200);
        Gizmos.DrawWireSphere(pivotOffset2.position, 200);
    }
}