using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class maxsGoToSceneCode : NetworkBehaviour
{

    public int nextScene;
    public void goToScene()
    {
        NetworkManager.SceneManager.LoadScene("readyUpScreen", LoadSceneMode.Single);
        //SceneManager.LoadSceneAsync(nextScene);
    }
}
