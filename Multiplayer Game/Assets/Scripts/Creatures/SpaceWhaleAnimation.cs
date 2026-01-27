using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public class SpaceWhaleAnimation : MonoBehaviour
{
    [SerializeField] Transform[] objectChain = new Transform[5];
    List <float> followDistanceChain = new List<float>();
    Vector3 lastPosition;
    void Start()
    {
        CalcuateFollowDistances();
    }
    void FixedUpdate()
    {
        TurnObjects();
        FollowObjects();
    }
    void TurnObjects()
    {
        //rotates segment to look at the segment in front
        if (objectChain.Length >= 2)
        {
            for (int i = 0; i < objectChain.Length - 1; i++)
            {
                //quaternion lookDir = Quaternion.RotateTowards(objectChain[i].rotation, objectChain[i + 1].rotation, 20f);
                quaternion lookDir = Quaternion.LookRotation((objectChain[i].position - objectChain[i + 1].position).normalized);
                objectChain[i].rotation = lookDir;
            }
        }
    }
    void FollowObjects()
    {
        //sets segment positions to stay withing bounds.
        for (int i = objectChain.Length - 1; i > 0; i--)
        {
            float dist = (Vector3.Distance(objectChain[i - 1].position, objectChain[i].position));
            if (dist > followDistanceChain[i - 1])
            {
                for (int j = 0; Vector3.Distance(objectChain[i - 1].position, objectChain[i].position) > followDistanceChain[i - 1]; j++)
                {
                    objectChain[i -1].position = Vector3.MoveTowards(objectChain[i - 1].position, objectChain[i].position, 0.2f);
                    if (j > 40)
                    {
                        Debug.Log("TURN DOWN THE MOVESPEED YOU PSYCHO. YOU TRIGGERED THE LAG PROTECTION");
                        return;
                    }
                }
            }
        }
    }
    void CalcuateFollowDistances()
    {
        //sets follow distance for each object in chain based off start position
        for (int i = 0; i < objectChain.Length - 1; i++)
        {
            float dist = (Vector3.Distance(objectChain[i].position, objectChain[i + 1].position));
            //add calucated distance for segment to list
            followDistanceChain.Add(dist);
        }
    }
}
