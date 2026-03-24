using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SpaceWhaleTargetMannager : NetworkBehaviour
{
    [SerializeField] GameObject spaceWhaleObject;
    [SerializeField] Image rageMeterOutputUIImage;
    [SerializeField] float rageGainSpeed = 1;
    [SerializeField] float pasiveRageDrain = 0.2f;
    public static bool doRageCheck = true;
    [Header("Don't Edit, View Only")]
    [SerializeField] List<GameObject> playerList;
    [SerializeField] static float rageAmount = 0;
    List<Vector3> lastReportedPlayerPosition = new List<Vector3> {Vector3.zero, Vector3.zero};
    bool currentlyAttacking = false;
    static bool ran = false;
    void FixedUpdate()
    {
        if (doRageCheck)
        {
            CalculateRageGain();
            if (rageAmount > 0.98f && !currentlyAttacking)
            {
                currentlyAttacking = true;
            }
        }
        if (currentlyAttacking && !ran)
        {
            ran = true;
            EnterAttackState();
        }
    }
    void CalculateRageGain()
    {
        //adds to rage meter based off movement of each player
        for (int i = 0; i < playerList.Count; i++)
        {
            //calculates distace the player(s) have traveled since last check
            float moveDist = Vector3.Distance(lastReportedPlayerPosition[i], playerList[i].transform.position);
            //does not apply distance to rage meter if distace jump is too far (added to help during testing)
            if (moveDist > 1)
            {
                Debug.Log("(Space Whale Mannager) Large distance jump detected stopping value from being applied to rage meter");
            } else
            {
                //adds to rage meter based off the distace moved
                rageAmount += moveDist * 0.01f * rageGainSpeed;
            }
            //updates the last position of the player for next check
            lastReportedPlayerPosition[i] = playerList[i].transform.position;
        }
        //applies the pasive drain on rage meter
        rageAmount -= pasiveRageDrain;
        //clamps rage amount for UI filling
        rageAmount = ClampFloat(rageAmount);
        //updates rage meter
        rageMeterOutputUIImage.fillAmount = rageAmount;
    }
    void EnterAttackState()
    {
        //targets a random player (server side)
        TargetRandomPlayerRPC();
        //stops rage check from happening and maxes out rage meter
        doRageCheck = false;
        rageAmount = 1;
        //updates rage meter
        rageMeterOutputUIImage.fillAmount = rageAmount;
    }
    public static void ExitAttackState()
    {
        //tells space whale to return to its normal patrol
        SpaceWhaleAI.ResumeFigure8Patrol();
        //resets rage meter
        doRageCheck = true;
        rageAmount = 0;
        //resets check to see if the whale can go into attack state
        ran = false;
    }
    float ClampFloat(float floatToClamp)
    {
        if (floatToClamp > 1)
        {
            floatToClamp = 1;
        }
        else if (floatToClamp < 0)
        {
            floatToClamp = 0;
        }
        return floatToClamp;
    }
    [Rpc(SendTo.Server)]
    void TargetRandomPlayerRPC()
    {
        SpaceWhaleAI.TargetObject(playerList[Random.Range(0, playerList.Count)].transform, true);
    }
    void ReturnToF8Patrol()
    {
        SpaceWhaleAI.ResumeFigure8Patrol();
    }
    public void SetPlayerObjectInList(GameObject playerObject, int indexToAssignObjectTo)
    {
        //checks if assignment attempt is within bounds of list
        if (indexToAssignObjectTo > playerList.Count)
        {
            Debug.LogWarning("(Space Whale Mannager) Attempting to assign object to index outside bounds of array!");
        }
        else
        {
            playerList[indexToAssignObjectTo] = playerObject;
            //sets last player position to avoid large value jumps
            lastReportedPlayerPosition[indexToAssignObjectTo] = playerList[indexToAssignObjectTo].transform.position;
        }
    }
}
