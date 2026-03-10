using UnityEngine;

public class SpawnatSpawnpoint : MonoBehaviour
{
    private void OnLevelWasLoaded(int level)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        GetComponent<CharacterController>().enabled = true;

    }
}
