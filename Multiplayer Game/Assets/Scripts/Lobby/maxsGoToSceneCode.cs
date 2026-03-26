using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class maxsGoToSceneCode : NetworkBehaviour
{

    public string nextScene = "readyUpScreen";
    public void goToScene()
    {
        NetworkManager.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        //SceneManager.LoadSceneAsync(nextScene);
    }
    private void OnTriggerEnter(Collider other)
    {
        goToScene();
    }
}
