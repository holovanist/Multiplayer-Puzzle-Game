using TMPro;
using UnityEngine;

public class detriggerSlideIn : MonoBehaviour
{

    public TMP_Text text;
    public void set()
    {
        text.GetComponent<Animator>().SetBool("shouldSlideIn", false);
    }
    }
