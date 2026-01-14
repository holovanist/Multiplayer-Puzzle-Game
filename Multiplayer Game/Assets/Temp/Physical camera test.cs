using UnityEngine;

public class Physicalcameratest : MonoBehaviour
{
    //Option A: Camera can only take one picture at a time
    public RenderTexture texture;
    public Camera cam;
    private void Start()
    {
        RenderTexture test = new RenderTexture(texture);
        cam.targetTexture = test;
    }
}
