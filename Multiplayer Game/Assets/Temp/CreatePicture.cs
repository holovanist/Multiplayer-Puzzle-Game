using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreatePicture : MonoBehaviour
{
    [SerializeField] GameObject PicturePrefab;
    [SerializeField] GameObject Cam;
    [SerializeField] int MaxPictures = 5;
    List<GameObject> Pictures = new List<GameObject>();
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
            if(Pictures.Count < MaxPictures)
            {
                TakePicture();
            }
            else
            {
                Destroy(Pictures[0]);
                Pictures.RemoveAt(0);
                TakePicture();
            }
        }
    }
    public void TakePicture()
    { 
        Cam.transform.position = transform.position;
        Cam.transform.rotation = transform.rotation;
        GameObject Picture = Instantiate(PicturePrefab, transform.position, Quaternion.identity);
        Pictures.Add(Picture);
        Picture.GetComponent<NetworkObject>().Spawn();
        Picture.GetComponent<PhysicalCamera>().cam = Cam.GetComponent<Camera>();
        Picture.GetComponent<PhysicalCamera>().SetTexture();
    }
}
