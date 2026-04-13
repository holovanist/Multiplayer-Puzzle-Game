using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MultiplayerUI : NetworkBehaviour
{
    private UnityTransport unityTransport;
    NetworkManager networkManager;
    public TMP_InputField IPInput;
    public GameObject LANObject;
    public GameObject LANButton;
    public GameObject Camera;
    public GameObject StackCamera; 
    public string nextScene = "readyUpScreen";

    private void Start()
    {
        unityTransport = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<UnityTransport>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }
    public void LAN()
    {
        LANObject.SetActive(true);
        LANButton.SetActive(false);
    }
    public void Create()
    {
        unityTransport.SetConnectionData(IPInput.text, 7777);
        networkManager.StartHost();
        if(Camera != null )
        Camera.SetActive(false);
        NetworkManager.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        gameObject.SetActive(false);
    }
    public void Join()
    {
        unityTransport.SetConnectionData(IPInput.text, 7777);
        networkManager.StartClient();
        if(Camera != null )
        Camera.SetActive(false);
        NetworkManager.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        gameObject.SetActive(false);
    }
}
