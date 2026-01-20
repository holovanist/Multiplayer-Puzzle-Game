using Unity.Mathematics;
using UnityEngine;

public class SpaceWhaleAnimation : MonoBehaviour
{
    [SerializeField] float followDistance = 75f;
    [SerializeField] float maxMoveSpeed = 1f;
    [SerializeField] Transform followObject;
    [SerializeField] Transform spaceWhaleObject;
    [SerializeField] Transform bone1;
    [SerializeField] Transform bone2;
    [SerializeField] Transform bone3;
    [SerializeField] Transform bone4;
    Vector3 lastPosition;
    void Start()
    {
        RecalcuateHeadPosition();
    }
    void FixedUpdate()
    {
        if (followObject.position != lastPosition)
        {
            lastPosition = followObject.position;
            RecalcuateHeadPosition();
        }
    }
    void RecalcuateHeadPosition()
    {
        //calcuates rotation for bone1(root bone) to look at
        Quaternion lookRotation = Quaternion.LookRotation((followObject.position - bone1.position).normalized);
        bone1.rotation = lookRotation;
        //sets position to stay withing bounds.
        float dist = (Vector3.Distance(spaceWhaleObject.position, followObject.position));
        if (dist > followDistance)
        {
            if (dist >= followDistance * 1.2f)
            {
                spaceWhaleObject.position = followObject.position;
                Debug.Log("Distance too long, lag protection triggered! Lower Movespeed! Space whales DONT need to that fast you psycho!!!!");
            }
            else
            {
                for (int i = 0; Vector3.Distance(spaceWhaleObject.position, followObject.position) > followDistance; i++)
                {
                    spaceWhaleObject.position = Vector3.MoveTowards(spaceWhaleObject.position, followObject.position, 1f);
                }
            }
                
        }
    }
}
