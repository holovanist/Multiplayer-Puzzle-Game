using System;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    public bool InMonitor;
    public Camera MonitorCamera;
    public Image[] wantedItems;
    public Image OtherPlayersRequest;
    public Image selectedItem;
    public Request request;
    Camera Cam;
    public bool IsTop;
    public bool WantedItemsCanBeSet { get; set; } = false;
    private void Start()
    {
    }
    void Update()
    {
        if(WantedItemsCanBeSet)
        {
            SetWantedItems();
        }
        if (InMonitor)
            EnableCanvas();
        else if (!InMonitor && Cam != null && Cam.enabled == false)
        {
            DisableCanvas();
        }
    }
    public void EnableCanvas()
    {
        Cam.enabled = false;
        MonitorCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void DisableCanvas()
    {
        Cam.enabled = true;
        MonitorCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void sendImageRPC()
    {
        //send image ItemID
        //set image based on ItemID
    }
    public void SetImage(int ItemID)
    {
        if(IsTop)
        {
            for (int i = 0; i < request.ListOfPotentialObject2.Length; i++)
            {
                if (request.ListOfPotentialObject2[i].ItemID == ItemID)
                {
                    selectedItem.sprite = request.ListOfPotentialObject2[i].ObjectPicture;
                }
            }
        }   
        else
        {
            for (int i = 0; i < request.ListOfPotentialObject1.Length; i++)
            {
                if (request.ListOfPotentialObject1[i].ItemID == ItemID)
                {
                    selectedItem.sprite = request.ListOfPotentialObject1[i].ObjectPicture;
                }
            }
        }
    }
    public void SetWantedItems()
    {
        if(IsTop)
        {
            for(int i = 0;i < wantedItems.Length;i++)
            {
                wantedItems[i].sprite = request.Side2ObjectPicture[i];
            }
        }
        else
        {
            for (int i = 0;i < wantedItems.Length; i++)
            {
                wantedItems[i].sprite = request.Side1ObjectPicture[i];
            }
        }
    }
    public void SetBool(Camera cam)
    {
        Cam = cam;
        InMonitor = true;
    }
}

[Serializable]
public class ObjectsToFeed
{
    public Sprite ObjectPicture;
    public GameObject GameObject;
    public int ItemID;
}
