using Unity.Netcode;
using UnityEngine;

public class Lever : NetworkBehaviour
{
    public bool pulled;
    Animator anim;
    public string animationTriggerUp;
    public string animationTriggerDown;
    public LeverController LC;
    public bool HoldLever;
    public float HoldTime;
    public bool Oppisite;
    public float timer {  get; set; }
    public bool LeverActive { get; set; }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void OnInteract()
    {
        timer += Time.deltaTime;
        if(!pulled)
        {
            if (!HoldLever)
            {
                pulled = true;
                LC.LeverStateChanged = true;
                if(!Oppisite) LeverActive = true;
                else LeverActive = false;
                if (anim != null)
                    anim.SetTrigger(animationTriggerDown);
            }
            else if (timer > HoldTime)
            {
                pulled = true;
                timer = 0;
                LC.LeverStateChanged = true;
                if (!Oppisite) LeverActive = true;
                else LeverActive = false;
                if (anim != null)
                    anim.SetTrigger(animationTriggerDown);
                return;
            }  
        }
        else
        {
            pulled = false;
            LC.LeverStateChanged = true;
            if (Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetTrigger(animationTriggerUp);
        }
    }
}
