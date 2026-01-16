using Unity.Mathematics;
using UnityEngine;

public class SpaceWhaleAnimation : MonoBehaviour
{
    [SerializeField] Transform followObject;
    [SerializeField] Transform bone1;
    [SerializeField] Transform bone2;
    [SerializeField] Transform bone3;
    [SerializeField] Transform bone4;
    Vector3 lastPosition;
    void Start()
    {
        RecalcuateHeadPosition();
    }
    void Update()
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


        //Vector3 correctedRotation = new Vector3(lookRotation.eulerAngles.x, lookRotation.eulerAngles.z, lookRotation.eulerAngles.y);
        //corrects rotation
        //bone1.localEulerAngles = new Vector3((bone1.localEulerAngles.x - 90), bone1.localEulerAngles.y, bone1.localEulerAngles.z);
    }
}
