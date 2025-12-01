using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : NetworkBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private string playerName;
    private bool host;
    private UnityTransport unityTransport;
    NetworkManager networkManager;

    private async void Start()
    {
        unityTransport = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<UnityTransport>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            //Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Player" + Random.Range(1, 99);
        //Debug.Log(playerName);
    }
    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }
    private async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    private async void HandleLobbyPollForUpdates()
    {
        if (hostLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.5f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }
    public async void CreateLobby()
    {
        try
        {
            host = true;
            CreateLobbyServer();
            string lobbyName = "My Lobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new()
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Gamemode1")},
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public, "Map1")}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = lobby;

            //Debug.Log("Created Lobby " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
     TextMeshProUGUI lobbyName;
     TextMeshProUGUI lobbyPlayers;
    public GameObject LobbyLocation;
    public GameObject LobbyPrefab;
    Transform[] q; 
    public async void ListLobbies()
    {
        try 
        {
            QueryLobbiesOptions queryLobbiesOptions = new()
            {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new(QueryFilter.FieldOptions.AvailableSlots, "0" , QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            q = LobbyLocation.GetComponentsInChildren<Transform>();
            if (q != null)
            foreach (Transform t2 in q)
            {
                //Destroy(t2.gameObject );
            }
            //Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
               /* GameObject l =  Instantiate(LobbyPrefab, LobbyLocation.transform);
                lobbyName = l.GetComponentsInChildren<TextMeshProUGUI>()[0];
                lobbyPlayers = l.GetComponentsInChildren<TextMeshProUGUI>()[1];
                lobbyName.text = lobby.Name;
                lobbyPlayers.text = lobby.Players.Count.ToString();*/
                //Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["PlayerName"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void Joinlobby()
    {
        try
        {
            /*            QueryLobbiesOptions queryLobbiesOptions = new()
                        {
                            Count = 25,
                            Filters = new List<QueryFilter> {
                                new(QueryFilter.FieldOptions.AvailableSlots, "0" , QueryFilter.OpOptions.GT)
                            },
                            Order = new List<QueryOrder>
                            {
                                new(false, QueryOrder.FieldOptions.Created)
                            }
                        };*/
            JoinLobbyByIdOptions joinLobbyByIdOptions = new()
            {
                Player = GetPlayer(),
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id, joinLobbyByIdOptions);
            joinedLobby = lobby;

            host = false;
            JoinLobby();
            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void JoinlobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new()
            {
                Player = GetPlayer(),
            };
            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            //Debug.Log("Joined Lobby with code " + lobbyCode);

            host = false;
            JoinLobby();
            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void QuickJoinLobby()
    {
        try
        { 
            await LobbyService.Instance.QuickJoinLobbyAsync();
            host = false;
            JoinLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName) },
                {"Ipv4", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,GetLocalIPv4()) },
                {"Host", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,host.ToString()) }
            }
        };
    }
    public void Players()
    {
        PrintPlayers(hostLobby);
    }
    private void PrintPlayers(Lobby lobby)
    {
        //Debug.Log("Players in Lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    public async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostLobby = await LobbyService.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
        .AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }

    public async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName) },
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await LobbyService.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public TextMeshProUGUI t;
    private void JoinLobby()
    {
            string ip = "127.0.0.1";
            foreach (Player player in joinedLobby.Players)
            {
                Debug.Log(player.Data["Host"].Value);
                if (player.Data["Host"].Value == "True")
                {
                    Debug.Log("2");
                    ip = player.Data["Ipv4"].Value;
                }
            }
            t.text = ip;
            Debug.Log(ip);
            unityTransport.SetConnectionData(ip, 7777);
            networkManager.StartClient();
    }
    public void CreateLobbyServer()
    {
        unityTransport.SetConnectionData(GetLocalIPv4(), 7777);
        networkManager.StartHost();
    }
}
