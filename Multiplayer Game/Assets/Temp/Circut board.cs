using Unity.Netcode;
using UnityEngine;

public class Circutboard : NetworkBehaviour
{
    public GameObject WirePart;
    public GameObject Door;
    public int ClockwiseRotationAmount = -45;
    public int CounterClockwiseRotationAmount = 45;
    int CurrentRotation;
    [Rpc(SendTo.Everyone)]
    public void RotateWireClockwiseRPC()
    {
        Debug.Log("test");
        WirePart.transform.Rotate(ClockwiseRotationAmount, WirePart.transform.rotation.y, WirePart.transform.rotation.z);
        Door.GetComponent<CircutBoardDoors>().StateUpdated = true;
    }
    [Rpc(SendTo.Everyone)]
    public void RotateWireCounterClockwiseRPC()
    {
        WirePart.transform.Rotate(CounterClockwiseRotationAmount, WirePart.transform.rotation.y, WirePart.transform.rotation.z);
        Door.GetComponent<CircutBoardDoors>().StateUpdated = true;
    }
}
