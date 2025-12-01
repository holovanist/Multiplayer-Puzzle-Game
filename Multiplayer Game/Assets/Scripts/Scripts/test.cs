using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public List<GameObject> a;
    public List<GameObject> b;
    public bool ab;
    bool c;

    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
            ab = true;
        if(Input.GetKeyDown(KeyCode.U))
            ab = false;
    


        if (ab)
        {
            if(!c)
            {
                a.Remove(GameObject.FindGameObjectWithTag("1"));
                c = true;
            }
            foreach (GameObject go in a)
            {
                go.SetActive(false);
            }
            foreach(GameObject go in b)
            {
            go.SetActive(true);
            }
        }
        else if (!ab)
        {
            foreach (GameObject go in b)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in a)
            {
                go.SetActive(true);
            }
        }
    }
}
