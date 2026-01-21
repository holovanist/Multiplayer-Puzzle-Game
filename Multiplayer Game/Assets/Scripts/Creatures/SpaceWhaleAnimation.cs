using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public class SpaceWhaleAnimation : MonoBehaviour
{
    [SerializeField] float followDistance = 75f;
    [SerializeField] Transform[] objectChain = new Transform[5];
    [SerializeField] Transform followObject;
    [SerializeField] Transform spaceWhaleObject;
    [SerializeField] Transform bone1;
    [SerializeField] Transform bone2;
    [SerializeField] Transform bone3;
    [SerializeField] Transform bone4;
    List <float> followDistanceChain;
    Vector3 lastPosition;
    void Start()
    {
        CalcuateFollowDistances();
        RecalcuateHeadPosition();
    }
    void FixedUpdate()
    {
        if (followObject.position != lastPosition)
        {
            lastPosition = followObject.position;
            TurnObjects();
            FollowObjects();
        }
    }
    void RecalcuateHeadPosition()
    {
        /*//calcuates rotation for bone1(root bone) to look at
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
                
        }*/
    }
    void TurnObjects()
    {
        if (objectChain.Length >= 2)
        {
            for (int i = 0; i < objectChain.Length - 1; i++)
            {
                quaternion lookDir = Quaternion.LookRotation((objectChain[i].position - objectChain[i + 1].position).normalized);



                objectChain[i].rotation = lookDir;
            }
        }
    }
    void FollowObjects()
    {
        //sets position to stay withing bounds.
        for (int i = 0; i < objectChain.Length - 1; i++)
        {
            float dist = (Vector3.Distance(objectChain[i].position, objectChain[i + 1].position));
            if (dist > followDistance)
            {
                for (int j = 0; Vector3.Distance(objectChain[i].position, objectChain[i + 1].position) > followDistance; j++)
                {
                    objectChain[i].position = Vector3.MoveTowards(objectChain[i].position, objectChain[i + 1].position, 0.05f);
                }
            }
        }

    }
    void CalcuateFollowDistances()
    {
        for (int i = 0; i < objectChain.Length - 1; i++)
        {
            float dist = (Vector3.Distance(objectChain[i].position, objectChain[i + 1].position));
            //followDistanceChain 
        }
    }
}
