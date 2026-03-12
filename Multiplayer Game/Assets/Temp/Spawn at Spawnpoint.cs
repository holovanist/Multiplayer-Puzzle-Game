using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnatSpawnpoint : MonoBehaviour
{
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        GetComponent<CharacterController>().enabled = false;
        if(GameObject.FindGameObjectWithTag("SpawnPoint") != null)
        transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position + new Vector3(Random.Range(0,4f), 0, Random.Range(0,4f));
        GetComponent<CharacterController>().enabled = true;
    }
}
