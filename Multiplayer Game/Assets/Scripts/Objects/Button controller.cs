using UnityEngine;
using Unity.Netcode;
public class ButtonController : NetworkBehaviour
{
    public Button[] buttons;
    public bool oppisite;
    Animator anim;
    public int NumberOfButtonsActive { get; set; }
    public int NumberOfButtonsDisabled { get; set; }
    public string animationTrigger;
    public bool ButtonStateChanged {  get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (buttons != null && ButtonStateChanged)
        {
            ButtonUpdaterRPC();
            ButtonStateChanged = false;
        }
    }

    public void ButtonUpdaterRPC()
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
                anim.SetBool(animationTrigger, true);
            else if(NumberOfButtonsActive < buttons.Length)
                anim.SetBool(animationTrigger, false);

        }
    }
}