using UnityEngine;

public class CreatePicture : MonoBehaviour
{
    [SerializeField] GameObject PicturePrefab;
    [SerializeField] GameObject Camera;
    [SerializeField] float TimePictureIsSpawned;
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
    }
}
