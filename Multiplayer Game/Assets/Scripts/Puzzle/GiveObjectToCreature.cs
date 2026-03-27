using UnityEngine;

public class GiveObjectToCreature : MonoBehaviour
{
    public Request request;
    [SerializeField] bool Side1;

    private void OnCollisionEnter(Collision collision)
    {
        CheckItems(collision);
    }
    void CheckItems(Collision collision)
    {
        if (request.Side1 == null) return;
        for (int i = 0; request.Side1.Length > 0; i++)
        {
            if (request.Side1[i].GameObject == collision.gameObject && Side1)
            {
                request.ObjectsGivenToCreature++;
                Destroy(collision.gameObject);
            }
            if (request.Side2[i].GameObject == collision.gameObject && !Side1)
            {
                request.ObjectsGivenToCreature++;
                Destroy(collision.gameObject);
            }
        }
    }
}
