using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour
{
    public string levelToLoad;
    public string sceneName = "scene 1";
    public GameObject sceneObject;

    private void Start()
    {
        sceneObject = GameObject.FindGameObjectWithTag(sceneName);
    }
    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        sceneObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        sceneObject.SetActive(false);
    }
}
