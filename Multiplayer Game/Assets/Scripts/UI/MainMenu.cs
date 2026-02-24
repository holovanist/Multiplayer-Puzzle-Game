using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public int nextScene;

    void Start()
    {

    }
    public void LoadHostSelect()
    {
        SceneManager.LoadSceneAsync(nextScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
