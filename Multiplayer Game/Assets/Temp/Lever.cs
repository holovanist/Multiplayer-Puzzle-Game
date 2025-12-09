using Unity.Netcode;
using UnityEngine;

public class Lever : NetworkBehaviour
{
    public bool pulled;
    Animator anim;
    public string animationTriggerUp;
    public string animationTriggerDown;
    public LeverController LC;
    public void OnInteract()
    {
        if(!pulled)
        {
            pulled = true;
            LC.LeverStateChanged = true;
            if (anim != null)
                anim.SetTrigger(animationTriggerDown);
        }
        else
        {
            pulled = false;
            LC.LeverStateChanged = true;
            if (anim != null)
                anim.SetTrigger(animationTriggerUp);
        }
    }
}
