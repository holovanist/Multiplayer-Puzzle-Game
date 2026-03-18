using UnityEngine;
using Unity.Netcode;

public class InteractiveButtons : NetworkBehaviour 
{
    public Light Light;
    public AudioSource Sound;
    public AudioClip AudioClip;
    public Lever[] Levers;
    public Button Button;
    public bool CanPlayAudio;
    public bool CanEnableLight;
    public int NumberOfLeversActive { get; set; }
    public int NumberOfLeversDisabled { get; set; }
    public bool LeverStateChanged { get; set; }
    private void Start()
    {
        Light.enabled = false;
    }
    float AudioCooldown;
    private void Update()
    {
        AudioCooldown += Time.deltaTime;
        if (Levers != null && LeverStateChanged || Button != null && LeverStateChanged)
        {
            LeverUpdater();
        }
    }

    public void LeverUpdater()
    {
        NumberOfLeversDisabled = 0;
        NumberOfLeversActive = 0;
        if(Button != null)
        {
            if (Button.ButtonsActive == true)
            {
                NumberOfLeversActive = 1;
            }
            if (Button.ButtonsActive == false)
            {
                NumberOfLeversDisabled = 1;
            }
            if (NumberOfLeversActive == 1 && AudioCooldown >= 1f)
            {
                LeverEnabledRpc();
            }
            else if (NumberOfLeversDisabled <= 1)
            {
                if (CanEnableLight)
                {
                    LeverDisabledRpc();
                }
            }
        }
        else
        {
            for (int i = 0; i < Levers.Length; i++)
            {
                if (Levers[i].LeverActive == true)
                {
                    NumberOfLeversActive++;
                }
                if (Levers[i].LeverActive == false)
                {
                    NumberOfLeversDisabled++;
                }
            }
            if (NumberOfLeversActive == Levers.Length && AudioCooldown >= 1f)
            {
                LeverEnabledRpc();
            }
            else if (NumberOfLeversDisabled <= Levers.Length)
            {
                if (CanEnableLight)
                {
                    LeverDisabledRpc();
                }
            }
        }
        LeverStateChanged = false;
    }
    [Rpc(SendTo.Everyone)]
    public void LeverEnabledRpc()
    {
        if (CanEnableLight && CanPlayAudio)
        {
            Light.enabled = true;
            Sound.PlayOneShot(AudioClip);
        }
        else if (CanPlayAudio && !CanEnableLight)
        {
            Sound.PlayOneShot(AudioClip);
        }
        else if (CanEnableLight && !CanPlayAudio)
        {
            Light.enabled = true;
        }
        AudioCooldown = 0;
    }
    [Rpc(SendTo.Everyone)]
    public void LeverDisabledRpc()
    {
        Light.enabled = false;
    }
}
