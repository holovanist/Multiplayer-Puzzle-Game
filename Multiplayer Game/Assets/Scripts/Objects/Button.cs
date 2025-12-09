using UnityEngine;

public class Button : MonoBehaviour
{
    public bool pressed;
    Animator anim;
    public string animationTriggerUp;
    public string animationTriggerDown;
    public ButtonController BC;
    int ObjectsOnButton;
    //update to use Raycast instead
    private void OnCollisionEnter(Collision collision)
    {
        ObjectsOnButton++;
        BC.buttonStateChanged = true;
        pressed = true;
        if (anim != null)
            anim.SetTrigger(animationTriggerDown);
    }
    private void OnCollisionExit(Collision collision)
    {
        ObjectsOnButton--;
        if (ObjectsOnButton == 0 )
        {
            BC.buttonStateChanged = true;
            pressed = false;
            if (anim != null)
                anim.SetTrigger(animationTriggerUp);
        }
    }
}