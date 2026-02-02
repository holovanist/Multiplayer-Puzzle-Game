using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreatePicture : NetworkBehaviour
{
    [SerializeField] GameObject PicturePrefab;
    [SerializeField] GameObject Cam;
    [SerializeField] int MaxPictures = 5;
    List<GameObject> Pictures = new();
    StoredObjectData StoredData;
    PlayerInput PlayerInput;
    private void Start()
    {
        StoredData= GetComponent<StoredObjectData>();
    }
    private void Update()
    {
        if(StoredData.IsHeld && PlayerInput == null)
        {
            PlayerInput = GetComponentInParent<PlayerInput>();
        }
        if(PlayerInput != null && PlayerInput.actions.FindAction("Left Click").WasPressedThisFrame() && StoredData.IsHeld)
        {
            if(Pictures.Count > MaxPictures)
            {
                Destroy(Pictures[0]);
                Pictures.RemoveAt(0);
            }
            TakePictureRpc();
        }
    }
    GameObject Picture;
    [Rpc(SendTo.Owner)]
    public void TakePictureRpc()
    {
        Cam.transform.SetPositionAndRotation(transform.position, transform.rotation);
        Picture = Instantiate(PicturePrefab, transform.position, Quaternion.identity);
        Pictures.Add(Picture);
        SpawnObjectRpc();
        Picture.GetComponent<PhysicalCamera>().cam = Cam.GetComponent<Camera>();
        Picture.GetComponent<PhysicalCamera>().SetTextureRPC();
    }
    [Rpc(SendTo.Server)]
    public void SpawnObjectRpc()
    { 
        Picture.GetComponent<NetworkObject>().Spawn();
    }
}
