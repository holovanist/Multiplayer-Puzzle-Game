using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class MultiplayerUI : MonoBehaviour
{
    private UnityTransport unityTransport;
    NetworkManager networkManager;
    public TMP_InputField IPInput;
    public GameObject LANObject;
    public GameObject OnlineObject;    
    public GameObject LANButton;
    public GameObject OnlineButton;
    public GameObject Camera;
    public GameObject StackCamera;

    private void Start()
    {
        unityTransport = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<UnityTransport>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }
    public void LAN()
    {
        LANObject.SetActive(true);
        LANButton.SetActive(false);
        OnlineButton.SetActive(false);
    }
    public void Online()
    {
        OnlineObject.SetActive(true);
        LANButton.SetActive(false);
        OnlineButton.SetActive(false);
    }
    public void Create()
    {
        unityTransport.SetConnectionData(IPInput.text, 7777);
        networkManager.StartHost();
        Camera.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Join()
    {
        unityTransport.SetConnectionData(IPInput.text, 7777);
        networkManager.StartClient();
        Camera.SetActive(false);
        gameObject.SetActive(false);
    }
}
