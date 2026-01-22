using UnityEngine;

public class SpaceWhaleAI : MonoBehaviour
{
    [SerializeField] Transform SWFollowObject;
    [SerializeField] Transform PivotOffset1;
    [SerializeField] Transform PivotOffset2;
    bool state1 = true;
    bool state2 = false;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        PivotOffset1.eulerAngles = new Vector3(0, PivotOffset1.eulerAngles.y + 1, 0);
        if (PivotOffset1.eulerAngles.y == 0 && state1)
        {

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PivotOffset1.position, 200);
        Gizmos.DrawWireSphere(PivotOffset2.position, 200);
    }
}