using System.Linq;
using System.Net;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class SendIpThroughLobby : MonoBehaviour
{
    UnityTransport transport;
    private void Start()
    {
        SetConnectionAdress();
    }
    public void SetConnectionAdress()
    {
        transport = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<UnityTransport>();
        transport.SetConnectionData(GetLocalIPv4(), 7777);
    }
    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
        .AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }
}
