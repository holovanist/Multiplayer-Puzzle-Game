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
        if (request.Side1GameObject == null) return;
        for (int i = 0; request.Side1GameObject.Length > 0; i++)
        {
            if (request.Side1GameObject[i] == collision.gameObject && Side1)
            {
                request.ObjectsGivenToCreature++;
                Destroy(collision.gameObject);
            }
            if (request.Side2ObjectPicture[i] == collision.gameObject && !Side1)
            {
                request.ObjectsGivenToCreature++;
                Destroy(collision.gameObject);
            }
        }
    }
}
