using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class readyUp : MonoBehaviour
{

    public GameObject readyUpText;

    public bool readyOrNot = false;

    public void letsReadyUp()
    {
        if (readyOrNot)
        {
            readyUpText.GetComponent<TMP_Text>().text = "NOT READY";
            readyOrNot = false;
        }
        else
        {
            readyUpText.GetComponent<TMP_Text>().text = "READY";
            readyOrNot = true;
        }

        
    }
}
