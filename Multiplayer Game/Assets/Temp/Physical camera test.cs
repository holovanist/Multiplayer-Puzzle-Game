using UnityEngine;

public class Physicalcameratest : MonoBehaviour
{
    //Option A: Camera can only take one picture at a time
    public RenderTexture texture;
    public Material material;
    public Camera cam;
    private void Start()
    {
        RenderTexture test = new RenderTexture(texture);
        cam.targetTexture = test;
        Material mat = new Material(material);
        mat.mainTexture = test;
        GetComponent<MeshRenderer>().material = mat;
        Material mymat = GetComponent<Renderer>().material;
        mymat.SetTexture("_EmissionMap", test);
    }
}
