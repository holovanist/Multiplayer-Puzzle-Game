using UnityEngine;

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
    public void SetBool(Camera cam)
    {
        Cam = cam;
        InMonitor = true;
    }
}
