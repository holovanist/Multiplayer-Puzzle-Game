using UnityEngine;

public class Tunnel : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;
    public float speed;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(Point1 == other.gameObject)
        {
            Vector3.MoveTowards(transform.position, Point2.position, speed * Time.deltaTime);
        }
        if(Point2 == transform)
        {
            Vector3.MoveTowards(transform.position, Point1.position, speed * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
