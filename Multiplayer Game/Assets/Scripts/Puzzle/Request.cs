using System.Collections.Generic;
using UnityEngine;

public class Request : MonoBehaviour
{
    public GameObject[] ListOfPotentialObject1;
    public GameObject[] ListOfPotentialObject2;
    GameObject[] Side1 = new GameObject[3];
    GameObject[] Side2 = new GameObject[3];
    int[] SelectedObject1 = new int[3];
    int[] SelectedObject2 = new int[3];
    public bool PuzzleReset;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SelectedObject1[i] = (Random.Range(0, ListOfPotentialObject1.Length));
        }
        for (int i = 0; i < 3; i++)
        {
            SelectedObject2[i] = (Random.Range(0, ListOfPotentialObject2.Length));
        }
    }
    private void Update()
    {
        if (PuzzleReset)
            ResetPuzzle();
        if (Side1[0] == null || Side1[1] == null || Side1[2] == null || Side2[0] == null || Side2[1] == null || Side2[2] == null)
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
            if(SelectedObject2[0] != SelectedObject2[1] && SelectedObject2[0] != SelectedObject2[2] && SelectedObject2[1] != SelectedObject2[2])
            {
                for (int i = 0; i < SelectedObject2.Length; i++)
                {
                    Side2[i] = ListOfPotentialObject2[SelectedObject2[i]];
                }
            }
            if(SelectedObject1[0] != SelectedObject1[1] && SelectedObject1[0] != SelectedObject1[2] && SelectedObject1[1] != SelectedObject1[2])
            {
                for (int i = 0; i < SelectedObject1.Length; i++)
                {
                    Side1[i] = ListOfPotentialObject1[SelectedObject1[i]];
                }
            }
        }
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
