using Unity.Netcode;
using UnityEngine;

public class Lever : NetworkBehaviour
{
    [Header("References")]
    Animator anim;
    public LeverController LC;
    public string animationTriggerUp;
    public string animationTriggerDown;
    [Header("Button")]
    public bool IsButton;
    public bool WasPulled;
    float ButtonCountdown;
    float BasicButtonTimer;
    public float BasicButtonTime;
    [Range(0, 480)] public float MaxButtonTime;
    [Header("Lever & Button")]
    public bool HoldLever;
    public float HoldTime;
    public bool Oppisite;
    public bool pulled;
    public float Timer {  get; set; }
    public bool LeverActive { get; set; }
    public bool Interact { get; set; } = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(!Interact && ButtonCountdown > 0 && HoldLever)
        {
            WasPulled = true;
            ButtonCountdown -= Time.deltaTime;
        }
        if(ButtonCountdown <= 0 && IsButton && HoldLever && WasPulled)
        {
            if (Oppisite) LeverActive = true;
            else LeverActive = false;
            LC.LeverStateChanged = true;
            WasPulled = false;
            pulled = false;
        }
        if(BasicButtonTimer > 0)
            BasicButtonTimer -= Time.deltaTime;
        if(BasicButtonTimer <= 0 && IsButton && !HoldLever && WasPulled)
        {
            if (Oppisite) LeverActive = true;
            else LeverActive = false;
            LC.LeverStateChanged = true; 
            WasPulled = false;
            pulled = false;
        }
    }
    public void OnInteract()
    {
        if(!WasPulled)
        Timer += Time.deltaTime;
        if (IsButton && HoldLever && !WasPulled)
            HoldButton();
        else if (!pulled && !WasPulled)
        {
            if (!HoldLever && !IsButton)
            {
                pulled = true;
                LC.LeverStateChanged = true;
                if (!Oppisite) LeverActive = true;
                else LeverActive = false;
                if (anim != null)
                    anim.SetTrigger(animationTriggerDown);
            }
            else if (Timer > HoldTime && !IsButton)
            {
                pulled = true;
                Timer = 0;
                LC.LeverStateChanged = true;
                if (!Oppisite) LeverActive = true;
                else LeverActive = false;
                if (anim != null)
                    anim.SetTrigger(animationTriggerDown);
                return;
            }
            else if (IsButton)
                Button();

        }
        else if(pulled && !WasPulled)
        {
            pulled = false;
            LC.LeverStateChanged = true;
            if (Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetTrigger(animationTriggerUp);
        }
    }
    void Button()
    {
        if (!HoldLever)
        {
            BasicButtonTimer = BasicButtonTime;
            pulled = true; 
            WasPulled = true;
            LC.LeverStateChanged = true;
            if (!Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetTrigger(animationTriggerDown);
        }
    }    
    void HoldButton()
    {
        if (HoldLever)
        {
            if(MaxButtonTime == 0)
            ButtonCountdown += Time.deltaTime;
            else if (ButtonCountdown < MaxButtonTime)
                ButtonCountdown += Time.deltaTime;
            else
            pulled = true;
            Timer = 0;
            LC.LeverStateChanged = true;
            if (!Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetTrigger(animationTriggerDown);
            return;
        }
    }
}
