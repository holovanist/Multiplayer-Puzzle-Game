using Unity.Netcode;
using UnityEngine;

public class PhysicalCamera : MonoBehaviour
{
    //Option A: Camera can only take one picture at a time
    public RenderTexture texture;
    public Material material;
    public Camera cam;
    [SerializeField] int MaterialNumber;
    [Rpc(SendTo.Everyone)]
    public void SetTextureRPC()
    {
        RenderTexture test = new(texture);
        cam.targetTexture = test;
        GetComponent<Renderer>().materials[MaterialNumber].mainTexture = test;
        Material mymat = GetComponent<Renderer>().materials[MaterialNumber];
        mymat.SetTexture("_EmissionMap", test);
    }
}
