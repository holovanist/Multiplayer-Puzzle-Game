using UnityEngine;

public class ShipDoorOpener : MonoBehaviour
{
    [SerializeField] Transform shipDoor;
    [SerializeField] float moveAmount = 2;
    [SerializeField] float doorSpeed = 0.1f;

    Vector3 doorClosedPosition;
    Vector3 doorOpenPosition;
    int playerCount;
    bool doorOpen = false;
    bool doorMoving = false;
    private void Start()
    {
        //sets positions for the closed and open door states
        doorClosedPosition = shipDoor.localPosition;
        doorOpenPosition = new Vector3(doorClosedPosition.x, doorClosedPosition.y, doorClosedPosition.z + moveAmount);
    }
    private void FixedUpdate()
    {
        //checks if door should be moving (bool gets updated in child statements)
        if (doorMoving)
        {
            if (doorOpen)
            {
                //opens door and stops when it gets to the limits
                shipDoor.localPosition = Vector3.MoveTowards(shipDoor.localPosition, doorOpenPosition, doorSpeed);
                if (Vector3.Distance(shipDoor.localPosition, doorOpenPosition) <= 0.005)
                {
                    doorMoving = false;
                }
            }
            else
            {
                //closes door and stops when it gets to the limits
                shipDoor.localPosition = Vector3.MoveTowards(shipDoor.localPosition, doorClosedPosition, doorSpeed);
                if (Vector3.Distance(shipDoor.localPosition, doorClosedPosition) <= 0.005)
                {
                    doorMoving = false;
                }
            }
        }
        
    }
    void OpenDoor()
    {
        //makes sure door can't get opened when already open
        if (!doorOpen)
        {
            doorOpen = true;
            doorMoving = true;
        }
    }
    void CloseDoor()
    {
        doorOpen = false;
        doorMoving = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //when triggered by a player, increments player count, and tries to open door.
        if (other.tag == "PlayerObject")
        {
            playerCount ++;
            OpenDoor();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //subtracts from player count on exit
        if (other.tag == "PlayerObject")
        {
            playerCount--;
        }
        //if player count is 0 closes door
        if (playerCount <= 0)
        {
            CloseDoor();
            //makes sure that the count does not go below 0 somehow. idk how that would happen, but just in case lol ;3
            playerCount = 0;
        }
    }
}
