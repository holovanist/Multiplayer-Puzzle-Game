using UnityEngine;
using UnityEngine.SceneManagement;

public class maxsGoToSceneCode : MonoBehaviour
{

    public int nextScene;
    public void goToScene()
    {
        SceneManager.LoadSceneAsync(nextScene);
    }
}
