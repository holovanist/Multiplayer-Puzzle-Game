using System;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    public bool InMonitor;
    public Transform CameraPosition;
    Camera Cam;
    void Start()
    {
        
    }

    void Update()
    {
        if (InMonitor)
            MoveCamera(Cam);
    }
    public void MoveCamera(Camera pos)
    {
        if(Vector3.Distance(pos.transform.position, CameraPosition.position) > 0.2f)
        {
            Cam.GetComponentInParent<PlayerMovement>().enabled = false;
            Vector3.Lerp(pos.transform.position, CameraPosition.position, .1f);
        }
        else
        {
            pos.transform.position = CameraPosition.position;
        }
    }
    public void sendImageRPC()
    {
        //send image ItemID
        //set image based on ItemID
    }
    public void SetImage(int ItemID)
    {
        //set item as selected based on ItemID
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
    public Image ObjectPicture { get; set; }
    public GameObject GameObject { get; set; }
    public int ItemID { get; set; }
}
