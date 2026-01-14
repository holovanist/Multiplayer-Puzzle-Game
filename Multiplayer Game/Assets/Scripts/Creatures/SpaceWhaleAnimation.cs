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
        lastPosition = followObject.position;
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

    }
}
