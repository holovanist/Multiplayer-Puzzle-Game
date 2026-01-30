using TMPro;
using UnityEngine;

public class detriggerSlideIn : MonoBehaviour
{

    public GameObject text;
    public void set()
    {
        text.GetComponent<Animator>().SetBool("shouldSlideIn", false);
    }
    }
