using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class allobjectsinscene : MonoBehaviour
{
    public string nextScene = "Scene 2";
    public string sceneName = "scene 1";
    public GameObject sceneObject;
    public GameObject nextSceneObject;
    public string levelToLoad;
    void Update()
    {
        if (sceneObject == null)
            sceneObject = GameObject.FindGameObjectWithTag(sceneName);
        if (nextSceneObject == null)
            //OnSceneLoaded();
        if (Input.GetKeyUp(KeyCode.O))
        {
            sceneObject.SetActive(false);
        }
        if(Input.GetKeyUp(KeyCode.P))
        {
            sceneObject.SetActive(true);
            SceneManager.UnloadSceneAsync(levelToLoad);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        sceneObject.SetActive(false);
        nextSceneObject.SetActive(true);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneObject = GameObject.FindWithTag("Scene 1");
    }

}
