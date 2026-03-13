using Unity.Netcode;
using UnityEngine;

public class LeverController : NetworkBehaviour
{
    public Lever[] Levers;
    Animator anim;
    public int NumberOfLeversActive { get; set; }
    public int NumberOfLeversDisabled { get; set; }
    public string animationTrigger;
    public bool LeverStateChanged { get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Levers != null && LeverStateChanged)
        {
            LeverUpdaterRPC();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void LeverUpdaterRPC()
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
        if (anim != null)
        {
            if (NumberOfLeversActive == Levers.Length)
                anim.SetBool(animationTrigger, true);
            else if (NumberOfLeversDisabled <= Levers.Length)
                anim.SetBool(animationTrigger, false);

        }
        LeverStateChanged = false;
    }
}
