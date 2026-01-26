using Unity.VisualScripting;
using UnityEngine;

public class SpaceWhaleAI : MonoBehaviour
{
    [SerializeField] Transform SWFollowObject;
    [SerializeField] Transform PivotFollowObject;
    [SerializeField] Transform PivotOffset1;
    [SerializeField] Transform PivotOffset2;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] bool run;
    bool state1 = true;
    bool canFlipState = false;
    void FixedUpdate()
    {
        if (run)
        {
            Figure8Patrol();
            MoveWhale();
        }
    }
    void Figure8Patrol()
    {
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
        if (state1)
        {
            state1 = false;
            PivotFollowObject.parent = PivotOffset2;
        } else
        {
            state1 = true;
            PivotFollowObject.parent = PivotOffset1;
        }
        PivotOffset1.eulerAngles = Vector3.zero;
        PivotOffset2.eulerAngles = Vector3.zero;
        Debug.Log("Switched State");
    }
    void MoveWhale()
    {
        SWFollowObject.position = Vector3.MoveTowards(SWFollowObject.position, PivotFollowObject.position, moveSpeed);
        //Debug.Log(Vector3.Distance(SWFollowObject.position, PivotFollowObject.position));
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PivotOffset1.position, 200);
        Gizmos.DrawWireSphere(PivotOffset2.position, 200);
    }
}