using Unity.Netcode;
using UnityEngine;

public class Lever : NetworkBehaviour
{
    [Header("References")]
    Animator anim;
    public LeverController[] LC;
    public InteractiveButtons IB;
    public string animationActive;

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
            if (LC != null)
            {
                for(int i = 0; i < LC.Length; i++)
                {
                    LC[i].LeverStateChanged = true;
                }
            }
            if (IB != null) IB.LeverStateChanged = true;
            WasPulled = false;
            pulled = false;
        }
        if(BasicButtonTimer > 0)
            BasicButtonTimer -= Time.deltaTime;
        if(BasicButtonTimer <= 0 && IsButton && !HoldLever && WasPulled)
        {
            if (Oppisite) LeverActive = true;
            else LeverActive = false;
            if(LC != null)
            {
                for (int i = 0; i < LC.Length; i++)
                {
                    LC[i].LeverStateChanged = true;
                }
            }
            if (IB != null) IB.LeverStateChanged = true;
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
                if (LC != null)
                {
                    for (int i = 0; i < LC.Length; i++)
                    {
                        LC[i].LeverStateChanged = true;
                    }
                }
                if (IB != null) IB.LeverStateChanged = true;
                if (!Oppisite) LeverActive = true;
                else LeverActive = false;
                if (anim != null)
                    anim.SetBool(animationActive, false);
            }
            else if (Timer > HoldTime && !IsButton)
            {
                pulled = true;
                Timer = 0;
                if (LC != null)
                {
                    for (int i = 0; i < LC.Length; i++)
                    {
                        LC[i].LeverStateChanged = true;
                    }
                }
                if (IB != null) IB.LeverStateChanged = true;
                if (!Oppisite) LeverActive = true;
                else LeverActive = false;
                if (anim != null)
                    anim.SetBool(animationActive, false);
                return;
            }
            else if (IsButton)
                Button();

        }
        else if(pulled && !WasPulled)
        {
            pulled = false;
            if (LC != null) 
            {                
                for (int i = 0; i < LC.Length; i++)
                {
                    LC[i].LeverStateChanged = true;
                }
            }
            if (IB != null) IB.LeverStateChanged = true;
            if (Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetBool(animationActive, true);
        }
    }
    void Button()
    {
        if (!HoldLever)
        {
            BasicButtonTimer = BasicButtonTime;
            pulled = true; 
            WasPulled = true;
            if (LC != null)
            {
                for (int i = 0; i < LC.Length; i++)
                {
                    LC[i].LeverStateChanged = true;
                }
            }
            if (IB != null) IB.LeverStateChanged = true;
            if (!Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetBool(animationActive, false);
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
            if (LC != null)
            {
                for (int i = 0; i < LC.Length; i++)
                {
                    LC[i].LeverStateChanged = true;
                }
            }
            if (IB != null) IB.LeverStateChanged = true;
            if (!Oppisite) LeverActive = true;
            else LeverActive = false;
            if (anim != null)
                anim.SetBool(animationActive, false);
            return;
        }
    }
}
