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
    private void OnCollisionEnter(Collision collision)
    {
        ObjectsOnButton++;
        pressed = true;
        if (!Oppisite) ButtonsActive = true;
        else ButtonsActive = false;
        BC.ButtonStateChanged = true;
        if (anim != null)
            anim.SetTrigger(animationTriggerDown);
    }
    private void OnCollisionExit(Collision collision)
    {
        ObjectsOnButton--;
        if (ObjectsOnButton == 0)
        {
            pressed = false;
            if (Oppisite) ButtonsActive = true;
            else ButtonsActive = false;
            BC.ButtonStateChanged = true;
            if (anim != null)
                anim.SetTrigger(animationTriggerUp);
        }
    }    
    private void OnTriggerEnter(Collider collider)
    {
        ObjectsOnButton++;
        pressed = true;
        if (!Oppisite) ButtonsActive = true;
        else ButtonsActive = false;
        BC.ButtonStateChanged = true;
        if (anim != null)
            anim.SetTrigger(animationTriggerDown);
    }
    private void OnTriggerExit(Collider collider)
    {
        ObjectsOnButton--;
        if (ObjectsOnButton == 0)
        {
            pressed = false;
            if (Oppisite) ButtonsActive = true;
            else ButtonsActive = false;
            BC.ButtonStateChanged = true;
            if (anim != null)
                anim.SetTrigger(animationTriggerUp);
        }
    }
}