using UnityEngine;

public class StoredObjectData : MonoBehaviour
{
    public bool isPickupable = false;
    public bool canPressButtons = false;
    public bool spinMeRoundBabyRightRound = false;
    public bool ChangeRotation = false;
    public bool IsHeld {  get; set; }
    public bool IsInTunnel {  get; set; }
    public int ID { get; set; } = 0;
    public string ObjectName;

    public Quaternion RotationOffset;
}
