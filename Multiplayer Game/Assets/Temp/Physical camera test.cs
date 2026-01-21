using UnityEngine;

public class Physicalcameratest : MonoBehaviour
{
    //Option A: Camera can only take one picture at a time
    public RenderTexture texture;
    public Material material;
    public Camera cam;
    [SerializeField] int MaterialNumber;
    private void Start()
    {
        RenderTexture test = new(texture);
        cam.targetTexture = test;
        GetComponent<Renderer>().materials[MaterialNumber].mainTexture = test;
        Material mymat = GetComponent<Renderer>().materials[MaterialNumber];
        mymat.SetTexture("_EmissionMap", test);
    }
}
