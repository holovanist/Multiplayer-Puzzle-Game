using Unity.Netcode;
using UnityEngine;

public class Request : NetworkBehaviour
{
    public ObjectsToFeed[] ListOfPotentialObject1;
    public ObjectsToFeed[] ListOfPotentialObject2;
    public ObjectsToFeed[] Side1 = new ObjectsToFeed[3];
    public ObjectsToFeed[] Side2 = new ObjectsToFeed[3];
    int[] SelectedObject1 = new int[3];
    int[] SelectedObject2 = new int[3];
    public bool PuzzleReset;
    public int ObjectsGivenToCreature {  get; set; }

    [SerializeField] Animator anim;
    [SerializeField] string PuzzleComplete;
    [SerializeField] string UncompletePuzzle;
    bool PuzzleDone;

    private void Start()
    {
        PuzzleDone = false;
        ObjectsGivenToCreature = 0;
        if(IsServer)
        {
            RandomizePuzzleRPC();
        }
    }
    private void Update()
    {
        if(ObjectsGivenToCreature == 6 && !PuzzleDone)
        {
            anim.SetTrigger(PuzzleComplete);
            PuzzleDone = true;
        }
        if(PuzzleDone && ObjectsGivenToCreature != 6)
        {
            anim.SetTrigger(UncompletePuzzle);
            PuzzleDone = false; 
        }
        if (PuzzleReset)
            ResetPuzzle();
        if (Side1[0] == null || Side1[1] == null || Side1[2] == null || Side2[0] == null || Side2[1] == null || Side2[2] == null)
        {
            if(IsServer)
            RandomizePuzzleCheckRPC();
            if (SelectedObject2[0] != SelectedObject2[1] && SelectedObject2[0] != SelectedObject2[2] && SelectedObject2[1] != SelectedObject2[2])
            {
                for (int i = 0; i < SelectedObject2.Length; i++)
                {
                    Debug.Log(Side2[1]);
                    Side2[i].ObjectPicture = ListOfPotentialObject2[SelectedObject2[i]].ObjectPicture;
                    Side2[i].GameObject = ListOfPotentialObject2[SelectedObject2[i]].GameObject;
                    Side2[i].ItemID = ListOfPotentialObject2[SelectedObject2[i]].ItemID;
                }
            }
            if (SelectedObject1[0] != SelectedObject1[1] && SelectedObject1[0] != SelectedObject1[2] && SelectedObject1[1] != SelectedObject1[2])
            {
                for (int i = 0; i < SelectedObject1.Length; i++)
                {
                    Side1[i].ObjectPicture = ListOfPotentialObject1[SelectedObject1[i]].ObjectPicture;
                    Side1[i].GameObject = ListOfPotentialObject1[SelectedObject1[i]].GameObject;
                    Side1[i].ItemID = ListOfPotentialObject1[SelectedObject1[i]].ItemID;
                }
            }
        }
    }
    [Rpc(SendTo.Server)]
    public void RandomizePuzzleRPC()
    {
        Debug.Log("server");
        for (int i = 0; i < 3; i++)
        {
            SelectedObject1[i] = (Random.Range(0, ListOfPotentialObject1.Length));
            //set requsted item images
        }
        for (int i = 0; i < 3; i++)
        {
            SelectedObject2[i] = (Random.Range(0, ListOfPotentialObject2.Length));
            //set requsted item images
        }
    }
    [Rpc(SendTo.Server)]
    public void RandomizePuzzleCheckRPC()
    {
        if (SelectedObject1[0] == SelectedObject1[1] || SelectedObject1[0] == SelectedObject1[2])
        {
            SelectedObject1[0] = Random.Range(0, ListOfPotentialObject1.Length);
        }
        else if (SelectedObject1[1] == SelectedObject1[2])
        {
            SelectedObject1[1] = Random.Range(0, ListOfPotentialObject1.Length);
        }
        if (SelectedObject2[0] == SelectedObject2[1] || SelectedObject2[0] == SelectedObject2[2])
        {
            SelectedObject2[0] = Random.Range(0, ListOfPotentialObject2.Length);
        }
        else if (SelectedObject2[1] == SelectedObject2[2])
        {
            SelectedObject2[1] = Random.Range(0, ListOfPotentialObject2.Length);
        }
        if (SelectedObject1[0] == SelectedObject1[1] || SelectedObject1[0] == SelectedObject1[2] || SelectedObject1[1] == SelectedObject1[2] || SelectedObject2[0] == SelectedObject2[1] || SelectedObject2[0] == SelectedObject2[2] || SelectedObject2[1] == SelectedObject2[2])
        {
            RandomizePuzzleCheckRPC();
        }
        else
        {
            RandomizePuzzleCheckClientRPC(SelectedObject1, SelectedObject2);
        }
    }
    [Rpc(SendTo.NotServer)]
    public void RandomizePuzzleCheckClientRPC(int[] side1, int[] side2)
    {
        SelectedObject1 = side1;
        SelectedObject2 = side2;
    }
    public void ResetPuzzle()
    {
        for(int i = 0;i < Side1.Length;i++)
        {
            SelectedObject1[i] = -1;
            SelectedObject2[i] = -1;
            Side1[i] = null;
            Side2[i] = null;
        }
        for (int i = 0; i < 3; i++)
        {
            SelectedObject1[i] = (Random.Range(0, ListOfPotentialObject1.Length));
        }
        for (int i = 0; i < 3; i++)
        {
            SelectedObject2[i] = (Random.Range(0, ListOfPotentialObject2.Length));
        }
        PuzzleReset = false;
    }
}
