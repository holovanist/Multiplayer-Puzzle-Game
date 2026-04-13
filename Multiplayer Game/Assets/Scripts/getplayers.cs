using UnityEngine;

public class getplayers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<SpaceWhaleTargetMannager>().GetPlayers();
    }
}
