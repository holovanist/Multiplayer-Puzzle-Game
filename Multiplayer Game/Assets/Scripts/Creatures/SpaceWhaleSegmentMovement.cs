using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SpaceWhaleSegmentMovement : MonoBehaviour
{
    [SerializeField] Transform[] objectChain = new Transform[4];
    [SerializeField] float followDistance = 2f;
    [SerializeField] AnimationCurve XAxisCurve = new AnimationCurve();
    [SerializeField] AnimationCurve YAxisCurve = new AnimationCurve();
    [SerializeField] AnimationCurve ZAxisCurve = new AnimationCurve();
    void Update()
    {
        if (objectChain != null)
        {
            TurnObjects();
            FollowObjects();
        }
    }
    void TurnObjects()
    {
        if (objectChain.Length >= 2)
        {
            for (int i = 0; i < objectChain.Length - 1; i++)
            {
                objectChain[i].rotation = Quaternion.LookRotation((objectChain[i].position - objectChain[i + 1].position).normalized);
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
}
