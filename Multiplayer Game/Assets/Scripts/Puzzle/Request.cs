using Unity.Netcode;
using UnityEngine;

public class Request : NetworkBehaviour
{
    public ObjectsToFeed[] ListOfPotentialObject1;
    public ObjectsToFeed[] ListOfPotentialObject2;
    public Sprite[] Side1ObjectPicture = new Sprite[3];
    public GameObject[] Side1GameObject = new GameObject[3];
    public int[] Side1ID = new int[3];
    public Sprite[] Side2ObjectPicture = new Sprite[3];
    public GameObject[] Side2GameObject = new GameObject[3];
    public int[] Side2ID = new int[3];
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
        if (Side1GameObject[0] == null || Side1GameObject[1] == null || Side1GameObject[2] == null || Side2GameObject[0] == null || Side2GameObject[1] == null || Side2GameObject[2] == null)
        {
            if(IsServer)
            RandomizePuzzleCheckRPC();
            if (SelectedObject2[0] != SelectedObject2[1] && SelectedObject2[0] != SelectedObject2[2] && SelectedObject2[1] != SelectedObject2[2])
            {
                for (int i = 0; i < SelectedObject2.Length; i++)
                {
                    Side2ObjectPicture[i] = ListOfPotentialObject2[SelectedObject2[i]].ObjectPicture;
                    Side2GameObject[i] = ListOfPotentialObject2[SelectedObject2[i]].GameObject;
                    Side2ID[i] = ListOfPotentialObject2[SelectedObject2[i]].ItemID;
                }
            }
            if (SelectedObject1[0] != SelectedObject1[1] && SelectedObject1[0] != SelectedObject1[2] && SelectedObject1[1] != SelectedObject1[2])
            {
                for (int i = 0; i < SelectedObject1.Length; i++)
                {
                    Side1ObjectPicture[i] = ListOfPotentialObject1[SelectedObject1[i]].ObjectPicture;
                    Side1GameObject[i] = ListOfPotentialObject1[SelectedObject1[i]].GameObject;
                    Side1ID[i] = ListOfPotentialObject1[SelectedObject1[i]].ItemID;
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
        Monitor[] monitors = FindObjectsByType<Monitor>(FindObjectsSortMode.None);
        for (int i = 0; i < monitors.Length; i++)
        {
            monitors[i].WantedItemsCanBeSet = true;
        }
    }
    [Rpc(SendTo.NotServer)]
    public void RandomizePuzzleCheckClientRPC(int[] side1, int[] side2)
    {
        SelectedObject1 = side1;
        SelectedObject2 = side2;
        Monitor[] monitors = FindObjectsByType<Monitor>(FindObjectsSortMode.None);
        for (int i = 0; i < monitors.Length; i++)
        {
            monitors[i].WantedItemsCanBeSet = true;
        }
    }
    public void ResetPuzzle()
    {
        for(int i = 0;i < Side1GameObject.Length;i++)
        {
            SelectedObject1[i] = -1;
            SelectedObject2[i] = -1;
            Side1GameObject[i] = null;
            Side1ID[i] = 0;
            Side1ObjectPicture[i] = null;
            Side2GameObject[i] = null;
            Side2ID[i] = 0;
            Side2ObjectPicture[i] = null;
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
