using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShipLauncher : MonoBehaviour
{
    [SerializeField] GameObject shipArmObject;
    [SerializeField] GameObject shipScreenMatSwapObject;
    [SerializeField] GameObject shipDoorTriggerObject;
    [SerializeField] GameObject whaleTerrainParentObject;
    [SerializeField] Material shipScreenMatSwap1;
    [SerializeField] Material shipScreenMatSwap2;
    [SerializeField] ShipDoorOpener shipDoorOpener;
    [SerializeField] float screenWarningBlinkInterval = 0.5f;
    [SerializeField] float shipArmRotateSpeed = 0.2f;
    [SerializeField] float shipArmRotateAmount = 90;
    [SerializeField] bool skipPlayerCountCheck = true;
    MeshRenderer screenMR;
    bool screenState = true;
    int currentFlightStep = 1;
    Vector3 startingPos;
    private void Start()
    {
        //gets mesh render for the main screen
        screenMR = shipScreenMatSwapObject.GetComponent<MeshRenderer>();
        startingPos = whaleTerrainParentObject.transform.position;
    }
    //main funtion that gets called to start the whole launch sequence
    public void StartShipLaunch()
    {
        if (CheckIfLaunchConditionsMet())
        {
            ShipScreenMaterialSwap();
            RotateShipArm();
            LockDoor();
            Invoke("ShipFlight", 10);
        }

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
        Invoke("ShipScreenMaterialSwap", screenWarningBlinkInterval);
    }
    void RotateShipArm()
    {
        //rotates ship arm and stops at set rotation amount
        shipArmObject.transform.eulerAngles = new Vector3 (shipArmObject.transform.eulerAngles.x, shipArmObject.transform.eulerAngles.y, shipArmObject.transform.eulerAngles.z + shipArmRotateSpeed);
        if (shipArmObject.transform.eulerAngles.z < shipArmRotateAmount)
        {
            Invoke("RotateShipArm", 0.02f);
        } else
        {
            shipArmObject.transform.eulerAngles = new Vector3(shipArmObject.transform.eulerAngles.x, shipArmObject.transform.eulerAngles.y, shipArmRotateAmount);
        }
    }
    void LockDoor()
    {
        //disables door opener code
        shipDoorOpener.enabled = false;
    }
    bool CheckIfLaunchConditionsMet()
    {
        if (skipPlayerCountCheck)
        {
            return true;
        } else
        {
            //place the code to check if both players are in the ship here!
            return false;
        }
        
    }
    void ShipFlight()
    {
        if (currentFlightStep == 1)
        {

        }
    }
    void ShipFlightX()
    {
        if (Vector3.Distance(startingPos, transform.position) >= 100)
        {

        }
    }
    void ShipFlightY()
    {
        
    }
}
