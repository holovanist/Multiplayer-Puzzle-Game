using UnityEngine;

public class TempShipLaunchActivator : MonoBehaviour
{
    [SerializeField] GameObject ShipObject;
    bool ran = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerObject" && !ran)
        {
            ran = true;
            Debug.Log("I ran :3");
            ShipObject.GetComponent<ShipLauncher>().StartShipLaunch();
        }
    }
}
