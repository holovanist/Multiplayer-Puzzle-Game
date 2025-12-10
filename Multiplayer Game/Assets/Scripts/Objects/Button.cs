using UnityEngine;

public class Button : MonoBehaviour
{
    public bool pressed;
    Animator anim;
    public string animationTriggerUp;
    public string animationTriggerDown;
    public ButtonController BC;
    int ObjectsOnButton;
    public bool Oppisite;
    public bool ButtonsActive {  get; set; }
    //update to use Raycast instead
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter()
    {
        ButtonInteraction();
    }
    private void OnCollisionExit()
    {
        ButtonInteraction();
    }    
    private void OnTriggerEnter()
    {
        ButtonInteraction();
    }
    private void OnTriggerExit()
    {
        ButtonInteraction();
    }
    void ButtonInteraction()
    {
        if (!pressed)
        {
            ObjectsOnButton--;
            if (ObjectsOnButton == 0)
            {
                BC.buttonStateChanged = true;
                pressed = false;
                if (!Oppisite) ButtonsActive = true;
                else ButtonsActive = false;
                if (anim != null)
                    anim.SetTrigger(animationTriggerUp);
            }
        }
        else
        {
            ObjectsOnButton++;
            BC.buttonStateChanged = true;
            pressed = true;
            if (!Oppisite) ButtonsActive = true;
            else ButtonsActive = false;
            if (anim != null)
                anim.SetTrigger(animationTriggerDown);
        }
    }
}