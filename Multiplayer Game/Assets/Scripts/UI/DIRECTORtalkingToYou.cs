using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DIRECTORtalkingToYou : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] sentences;

    public int currentSentance = 0;

    public TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.GetComponent<TMP_Text>().text = sentences[0];
      
    }

    public void TriggerDialogue()
    {
        

        currentSentance = currentSentance + 1;

        if(currentSentance >= sentences.Length)
        {
            SceneManager.LoadSceneAsync(0);
        }
        else
        {
            text.GetComponent<Animator>().SetBool("shouldSlideIn", true);

            text.GetComponent<TMP_Text>().text = sentences[currentSentance];
        }

        
    }
}
