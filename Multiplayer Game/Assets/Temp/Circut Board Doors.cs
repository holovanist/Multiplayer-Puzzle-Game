using UnityEngine;

public class CircutBoardDoors : MonoBehaviour
{
    public GameObject[] WireList;
    public bool StateUpdated { get; set; } = false;
    int WiresCorrect;
    public string AnimationBool;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(StateUpdated)
        {
            WiresCorrect = 0;
            for (int i = 0; i < WireList.Length; i++)
            {
                if (WireList[i].transform.rotation.x == -1 || WireList[i].transform.rotation.w == -1 || WireList[i].transform.rotation.x == 1 || WireList[i].transform.rotation.w == 1)
                {
                    WiresCorrect++;
                }
            }
            if(WiresCorrect >= WireList.Length)
            {
                anim.SetBool(AnimationBool, true);
            }
            else
                anim.SetBool(AnimationBool, false);
            StateUpdated = false;
        }
    }
}
