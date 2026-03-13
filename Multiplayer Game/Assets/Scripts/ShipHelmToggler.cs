using UnityEngine;

public class ShipHelmToggler : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] bool turnOn = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerObject")
        {
            targetObject.SetActive(turnOn);
        }
    }
}
