using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNextLevel : MonoBehaviour
{
    public Button[] buttons;
    public bool oppisite;
    Animator anim;
    public string LevelName;
    public int NumberOfButtonsActive { get; set; }
    public int NumberOfButtonsDisabled { get; set; }
    public string animationBool;
    public bool ButtonStateChanged { get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (buttons != null && ButtonStateChanged)
        {
            ButtonUpdater();
            ButtonStateChanged = false;
        }
    }

    public void ButtonUpdater()
    {
        NumberOfButtonsDisabled = 0;
        NumberOfButtonsActive = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].ButtonsActive == true)
            {
                NumberOfButtonsActive++;
            }
            if (buttons[i].ButtonsActive == false)
            {
                NumberOfButtonsDisabled++;
            }
        }
        if (anim != null)
        {
            SceneManager.LoadScene(LevelName);

        }
    }
}
