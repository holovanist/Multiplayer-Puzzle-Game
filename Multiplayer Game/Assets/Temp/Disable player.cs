using Unity.Netcode;
using UnityEngine;

public class Disableplayer : NetworkBehaviour
{
    void Update()
    {
        if(!IsLocalPlayer) return;
        if (GameObject.FindGameObjectWithTag("Disable Player") != null && GetComponent<PlayerMovement>().enabled == true)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(GameObject.FindGameObjectWithTag("Disable Player") == null && GetComponent<PlayerMovement>().enabled == false)
        {
            GetComponentInChildren<Camera>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if(Cursor.visible == false && GetComponent<PlayerMovement>().enabled == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
