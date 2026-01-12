using UnityEngine;

public class TunnelTravel : MonoBehaviour
{
    Tunnel tn; 
    public float speed;
    public bool MoveToPoint;
    private void OnTriggerEnter(Collider other)
    {
        if(!MoveToPoint)
        {
            MoveToPoint = true;
            tn = other.gameObject.GetComponentInParent<Tunnel>();
            tn.enterPoint = other.gameObject.transform;
        }
        else
        {
            MoveToPoint = false;
        }
    }
    private void Update()
    {
        if (MoveToPoint)
        {
            if (tn.Point1.position == tn.enterPoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, tn.Point2.position, speed * Time.deltaTime);
            }
            if (tn.Point2.position == tn.enterPoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, tn.Point1.position, speed * Time.deltaTime);
            }
        }
    }
}
