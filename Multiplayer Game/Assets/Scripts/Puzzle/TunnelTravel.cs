using UnityEngine;

public class TunnelTravel : MonoBehaviour
{
    Tunnel tn; 
    public float speed;
    public bool MoveToPoint;
    bool point1;
    Rigidbody rb;
    StoredObjectData SOD;
    [SerializeField] float pushMultiplier;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SOD = GetComponent<StoredObjectData>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tunnel")) return;
        if(!MoveToPoint)
        {
            MoveToPoint = true;
            tn = other.gameObject.GetComponentInParent<Tunnel>();
            if( tn != null)
            tn.enterPoint = other.gameObject.transform;
            SOD.IsInTunnel = true;
        }
        else 
        {
            MoveToPoint = false;
            SOD.IsInTunnel = false;
            if(point1 && tn != null)
            {
                dir = tn.Point2.position - tn.Point1.position;
                rb.AddForce(dir * pushMultiplier, ForceMode.Impulse);
            }            
            else if (!point1 && tn != null)
            {
                dir = tn.Point1.position - tn.Point2.position;
                rb.AddForce(dir * pushMultiplier, ForceMode.Impulse);
            }
        }
    }
    Vector3 dir;
    private void Update()
    {
        if (MoveToPoint && tn !=null && !SOD.IsHeld)
        {
            if (tn.Point1.position == tn.enterPoint.position)
            {
                point1 = true;
                transform.position = Vector3.MoveTowards(transform.position, tn.Point2.position, speed * Time.deltaTime);
            }
            if (tn.Point2.position == tn.enterPoint.position)
            {
                point1 = false;
                transform.position = Vector3.MoveTowards(transform.position, tn.Point1.position, speed * Time.deltaTime);
            }
        }
    }
}
