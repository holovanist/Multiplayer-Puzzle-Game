using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testscenetransfer : NetworkBehaviour
{
    public bool start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLocalPlayer)
        {
            if(start)
            {
                NetworkManager.SceneManager.LoadScene("Level 1 Split", LoadSceneMode.Single);
                start = false;
            }
        }
    }
}
