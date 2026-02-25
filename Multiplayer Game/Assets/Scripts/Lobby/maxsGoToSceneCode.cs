using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class maxsGoToSceneCode : NetworkBehaviour
{

    public int nextScene;
    public void goToScene()
    {
        NetworkManager.SceneManager.LoadScene("TestreadyUpScreen 1", LoadSceneMode.Single);
        //SceneManager.LoadSceneAsync(nextScene);
    }
}
