using UnityEngine;
using Unity.Netcode;

public class cameratest : NetworkBehaviour
{
    public GameObject cam; // Drag camera into here

    void Start()
    {
        if (IsOwner) return;

        cam.SetActive(false);
    }
}
