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
    PlayerInputHandler PlayerInput;
    private void Start()
    {
        StoredData= GetComponent<StoredObjectData>();
    }
    private void Update()
    {
        if(StoredData.IsHeld && PlayerInput == null)
        {
            PlayerInput = GetComponentInParent<PlayerInputHandler>();
        }
        if(PlayerInput != null && StoredData != null)
        {
            if(PlayerInput.playerControls.FindAction("Left Click").WasPressedThisFrame() && StoredData.IsHeld)
            {
                if(Pictures.Count > MaxPictures)
                {
                    DestroyObjectRpc(Pictures[0]);
                    Pictures.RemoveAt(0);
                }
                TakePictureRpc();
                if(Picture != null)
                Pictures.Add(Picture);
            }
        }
    }
    GameObject Picture;
    [Rpc(SendTo.Server)]
    public void TakePictureRpc()
    {
        Picture = Instantiate(PicturePrefab, transform.position, Quaternion.identity);
        Picture.GetComponent<NetworkObject>().Spawn();
        var targetObject = Picture.GetComponent<NetworkObject>();
        SetTextureRpc(targetObject);
    }
    [Rpc(SendTo.Everyone)]
    public void SetTextureRpc(NetworkObjectReference target)
    {
        if (Cam == null) Cam = GameObject.FindGameObjectWithTag("Physical Camera view");
        Cam.transform.SetPositionAndRotation(transform.position, transform.rotation);
        if (target.TryGet(out NetworkObject targetObject))
        {
            Picture = targetObject.gameObject;
            Picture.GetComponent<PhysicalCamera>().cam = Cam.GetComponent<Camera>();
            Picture.GetComponent<PhysicalCamera>().SetTextureRPC();
        }
    }
    [Rpc(SendTo.Server)]
    public void DestroyObjectRpc(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            Destroy(targetObject.gameObject);
        }
    }
}
