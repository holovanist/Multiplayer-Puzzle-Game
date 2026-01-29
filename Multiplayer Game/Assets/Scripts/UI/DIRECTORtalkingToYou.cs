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

    [SerializeField] public int evilVanish = 0;

    [SerializeField] public int turnMusicOff = 0;

    public TMP_Text text;

    public int nextScene;

    [SerializeField] public bool suddenVanish = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.GetComponent<TMP_Text>().text = sentences[0];

        GetComponent<Animator>().SetBool("appear", false);

        GetComponent<AudioSource>().volume = 1;

        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void TriggerDialogue()
    {
        

        currentSentance = currentSentance + 1;

        if(currentSentance >= sentences.Length)
        {
            SceneManager.LoadSceneAsync(nextScene);
        }
        else
        {
            text.GetComponent<Animator>().SetBool("shouldSlideIn", true);

            text.GetComponent<TMP_Text>().text = sentences[currentSentance];
        }

        if(currentSentance == 1)
        {
            GetComponent<Animator>().SetBool("appear", true);
        }

        if (currentSentance == evilVanish)
        {
            if (suddenVanish)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                GetComponent<Animator>().SetBool("appear", false);
            }

            
        }

        if (currentSentance == turnMusicOff)
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }
}
