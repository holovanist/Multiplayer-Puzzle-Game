using UnityEngine;

public class InteractiveButtons : MonoBehaviour 
{
    public Light Light;
    public AudioSource Sound;
    AudioClip AudioClip;
    public Lever[] Levers;
    public bool CanPlayAudio;
    public bool CanEnableLight;
    public int NumberOfLeversActive { get; set; }
    public int NumberOfLeversDisabled { get; set; }
    public bool LeverStateChanged { get; set; }
    private void Start()
    {
        Light.enabled = false;
    }
    private void Update()
    {
        if (Levers != null && LeverStateChanged)
        {
            LeverUpdater();
        }
    }

    public void LeverUpdater()
    {
        NumberOfLeversDisabled = 0;
        NumberOfLeversActive = 0;
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
        if (NumberOfLeversActive == Levers.Length)
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
        }
        else if (NumberOfLeversDisabled <= Levers.Length)
        {
            if (CanEnableLight)
            {
                Light.enabled = false;
            }
        }


        LeverStateChanged = false;
    }
}
