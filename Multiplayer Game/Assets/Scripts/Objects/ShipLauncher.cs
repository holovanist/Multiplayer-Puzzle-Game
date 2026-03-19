using UnityEngine;

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
    [SerializeField] float maxFlightSpeed = 0.5f;
    MeshRenderer screenMR;
    bool screenState = true;
    int currentFlightStep = 1;
    float lastSpeedUsed = 0.01f;
    bool useMaxSpeed = false;
    bool useMaxSpeed1 = false;
    private void Start()
    {
        //gets mesh render for the main screen
        screenMR = shipScreenMatSwapObject.GetComponent<MeshRenderer>();
        RenderSettings.skybox.SetFloat("_Rotation", 0);
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
    void ShipFlight()
    {
        //gets current terrain/whale parent object position
        Vector3 objectToMovePos = whaleTerrainParentObject.transform.position;
        //moves ship only on x until flightStep is set to 2 then moves on x & y
        if (currentFlightStep == 1)
        {
            ShipFlightForward(objectToMovePos);
        } else
        {
            ShipFlightForwardAndUp(objectToMovePos);
        }
        //calls itself and stops when flight step 3 is reached
        if (currentFlightStep != 3)
        {
            Invoke("ShipFlight", 0.02f);
        } else
        {
            //rolls credits when flight is done
            RollCredits();
        }
    }
    void ShipFlightForward(Vector3 objectPos)
    {
        //moves terrain back easing into the movement
        whaleTerrainParentObject.transform.position = new Vector3(objectPos.x - (useMaxSpeed ? maxFlightSpeed : SpeedEaseIn(lastSpeedUsed)), objectPos.y, objectPos.z);
        if (Vector3.Distance(transform.position, objectPos) >= 100)
        {
            currentFlightStep = 2;
            lastSpeedUsed = 0.01f;
        }
    }
    void ShipFlightForwardAndUp(Vector3 objectPos)
    {
        //moves terrain back and up easing into the vertical movement
        whaleTerrainParentObject.transform.position = new Vector3(objectPos.x - maxFlightSpeed, objectPos.y - (useMaxSpeed1 ? maxFlightSpeed : SpeedEaseIn(lastSpeedUsed)), objectPos.z);
        //stops movement when terrain is out of sight
        if (Vector3.Distance(transform.position, objectPos) >= 500)
        {
            currentFlightStep = 3;
        }
    }
    float SpeedEaseIn(float currentSpeed)
    {
        //starts at a slow speed and slowly ramps up to max
        float speedToUse = 0;
        if (currentSpeed < maxFlightSpeed)
        {
            speedToUse = currentSpeed * 1.01f;
        } else if (currentFlightStep == 1)
        {
            useMaxSpeed = true;
        } else if (currentFlightStep == 2)
        {
            useMaxSpeed1 = true;
        }
        lastSpeedUsed = speedToUse;
        return speedToUse;
    }
    bool CheckIfLaunchConditionsMet()
    {
        if (skipPlayerCountCheck)
        {
            return true;
        }
        else
        {
            //place the code to check if both players are in the ship here!
            return false;
        }
    }
    void RollCredits()
    {
        //fill with whatever is needed to roll the credits/trigger another function to so do, if you don't, I will just make it do a star-wars intro style roll of the entire bee movie script. :3
    }
}
