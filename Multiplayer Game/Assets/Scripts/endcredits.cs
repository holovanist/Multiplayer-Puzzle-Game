using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endcredits : NetworkBehaviour
{
    public void MainMenu()
    {
        ulong Id = NetworkManager.ConnectedClientsIds[0];
        ulong Id1 = NetworkManager.ConnectedClientsIds[1];
        SceneManager.LoadSceneAsync(1);
        NetworkManager.DisconnectClient(Id);
        NetworkManager.DisconnectClient(Id1);
    }
}
