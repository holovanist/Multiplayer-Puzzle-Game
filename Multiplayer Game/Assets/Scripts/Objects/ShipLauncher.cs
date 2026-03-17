using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShipLauncher : MonoBehaviour
{
    [SerializeField] GameObject shipArmObject;
    [SerializeField] GameObject shipScreenMatSwapObject;
    [SerializeField] GameObject shipDoorTriggerObject;
    [SerializeField] Material shipScreenMatSwap1;
    [SerializeField] Material shipScreenMatSwap2;
    [SerializeField] float ScreenWarningBlinkInterval = 0.5f;
    [SerializeField] float ShipArmRotateSpeed = 0.2f;
    [SerializeField] float ShipArmRotateAmount = 90;
    MeshRenderer screenMR;
    bool screenState = true;
    private void Start()
    {
        //gets mesh render for the main screen
        screenMR = shipScreenMatSwapObject.GetComponent<MeshRenderer>();
    }
    //main funtion that gets called to start the whole launch sequence
    public void StartShipLaunch()
    {
        ShipScreenMaterialSwap();
        RotateShipArm();
        LockDoor();
    }
    void ShipScreenMaterialSwap()
    {
        //switches between two provided screen materials
        if (screenState)
        {
            screenState = false;
            screenMR.material = shipScreenMatSwap1;
        }
        else
        {
            screenState = true;
            screenMR.material = shipScreenMatSwap2;
        }
        //Triggers itself at set interval
        Invoke("ShipScreenMaterialSwap", ScreenWarningBlinkInterval);
    }
    void RotateShipArm()
    {
        //rotates ship arm and stops at set rotation amount
        shipArmObject.transform.eulerAngles = new Vector3 (shipArmObject.transform.eulerAngles.x, shipArmObject.transform.eulerAngles.y, shipArmObject.transform.eulerAngles.z + ShipArmRotateSpeed);
        if (shipArmObject.transform.eulerAngles.z < ShipArmRotateAmount)
        {
            Invoke("RotateShipArm", 0.02f);
        } else
        {
            shipArmObject.transform.eulerAngles = new Vector3(shipArmObject.transform.eulerAngles.x, shipArmObject.transform.eulerAngles.y, ShipArmRotateAmount);
        }
    }
    void LockDoor()
    {

    }
}
