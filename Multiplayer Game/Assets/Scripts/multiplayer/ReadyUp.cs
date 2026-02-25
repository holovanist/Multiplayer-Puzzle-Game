using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyUp : NetworkBehaviour
{
    public NetworkVariable<int> State = new NetworkVariable<int>();
    [SerializeField]
    GameObject HostReadyIndicator;    
    [SerializeField]
    GameObject HostNotReadyIndicator;
    [SerializeField]
    GameObject ClientReadyIndicator;
    [SerializeField]
    GameObject ClientNotReadyIndicator;
    public string Level1;
    bool IsReady = false;
    private void Start()
    {
        if (!GetComponent<NetworkObject>().IsSpawned)
        SpawnRpc();
    }
    [Rpc(SendTo.Server)]
    void SpawnRpc()
    {
        GetComponent<NetworkObject>().Spawn();
    }
    public void Ready()
    {
        if(!IsReady && IsHost)
        {
            IsReadyUp(true, 1);
        }
        else if (IsReady && IsHost)
        {
            IsReadyUp(false, 1);
        }
        else if (!IsReady && !IsHost)
        {
            IsReadyUp(true, 2);
        }
        else if (IsReady && !IsHost)
        {
            IsReadyUp(false, 2);
        }
    }
    private void IsReadyUp(bool ready, int Player)
    {
        IsReady = ready;
        if(Player == 1)
        {
            if (ready)
            {
                ReadyRpc(ready, Player);
                ToggleServerRpc(1);
            }
            else if (!ready)
            {
                ReadyRpc(ready, Player);
                ToggleServerRpc(-1);
            }
            }
        if (Player == 2)
        {
            if (ready)
            {
                ReadyRpc(ready, Player);
                ToggleServerRpc(1);
            }
            else if (!ready)
            {
                ReadyRpc(ready, Player);
                ToggleServerRpc(-1);
            }
        }
    }
    [Rpc(SendTo.Everyone)]
    public void ReadyRpc(bool Ready, int Player)
    {
        if (Player == 1)
        {
            if (Ready)
            {
                HostReadyIndicator.SetActive(true);
                HostNotReadyIndicator.SetActive(false);
            }
            else if (!Ready)
            {
                HostReadyIndicator.SetActive(false);
                HostNotReadyIndicator.SetActive(true);
            }
        }
        if (Player == 2)
        {
            if (Ready)
            {
                ClientReadyIndicator.SetActive(true);
                ClientNotReadyIndicator.SetActive(false);
            }
            else if (!Ready)
            {
                ClientReadyIndicator.SetActive(false);
                ClientNotReadyIndicator.SetActive(true);
            }
        }
    }
    public override void OnNetworkSpawn()
    {
        State.OnValueChanged += OnSomeValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        State.OnValueChanged -= OnSomeValueChanged;
    }

    private void OnSomeValueChanged(int previous, int current)
    {
        if(current >= 2)
        {
            Debug.Log("Go to Next Scene");
            DeleteLobby();
            NetworkManager.SceneManager.LoadScene(Level1, LoadSceneMode.Single);
        }
    }

    [Rpc(SendTo.Server)]
    public void ToggleServerRpc(int NumberOfPlayersReady)
    {
        // this will cause a replication over the network
        // and ultimately invoke `OnValueChanged` on receivers
        State.Value += NumberOfPlayersReady;
    }
    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync("lobbyId");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
