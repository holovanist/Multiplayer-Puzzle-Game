using UnityEngine;

public class GiveObjectToCreature : MonoBehaviour
{
    Request request;
    [SerializeField] bool Side1;

    private void Start()
    {
        request = GetComponent<Request>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; request.Side1.Length > 0; i++)
        {
            if (request.Side1[i] == collision.gameObject && Side1)
            {
                request.ObjectsGivenToCreature++;
                Destroy(collision.gameObject);
            }
            if (request.Side2[i] == collision.gameObject && !Side1)
            {
                request.ObjectsGivenToCreature++;
                Destroy(collision.gameObject);
            }
        }    
    }
}
