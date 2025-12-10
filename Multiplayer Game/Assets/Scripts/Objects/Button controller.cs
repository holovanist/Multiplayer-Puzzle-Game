using UnityEngine;
using Unity.Netcode;
public class ButtonController : NetworkBehaviour
{
    public Button[] buttons;
    public bool oppisite;
    Animator anim;
    public int NumberOfButtonsActive { get; set; }
    public int NumberOfButtonsDisabled { get; set; }
    public string animationBool;
    public bool buttonStateChanged {  get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (buttons != null && buttonStateChanged)
        {
            ButtonUpdater();
        }
    }

    public void ButtonUpdater()
    {
        NumberOfButtonsDisabled = 0; 
        NumberOfButtonsActive = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].ButtonsActive == true)
            {
                NumberOfButtonsActive++;
            }
            if (buttons[i].ButtonsActive == false)
            {
                NumberOfButtonsDisabled++;
            }
        }
        if(anim != null)
        {
            if (NumberOfButtonsActive == buttons.Length)
                anim.SetBool(animationBool, true);
            else if(NumberOfButtonsActive < buttons.Length)
                anim.SetBool(animationBool, false);

        }
        buttonStateChanged = false;
    }
}