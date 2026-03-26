using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapePuzzle : NetworkBehaviour
{
    public List<ShapesToRandomize> ShapesToScroll;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public List<int> PuzzleObjects;
    int CorrectShapeCount;
    bool UpdateShape;
    Animator anim;
    public string animationTrigger;
    public TextMeshPro RequiredText;
    private void Start()
    {
        anim = GetComponent<Animator>(); 
    }
    private void Update()
    {
        if (!SpawnPuzzleObjects)
        {
            if (!IsServer)
            {
                SpawnPuzzleObjects = true;
                return;
            }
            SpawnPuzzleServerRPC();
            SpawnPuzzleObjects = true;
        }
        if(UpdateShape)
        {
            CorrectShapeCount = 0;
            for (int i = 0;i < PuzzleObjects.Count;i++)
            {
                if (ShapesToScroll[i].ObjectSpawned == PuzzleObjects[i])
                {
                    CorrectShapeCount++;
                }
            }
            UpdateShape = false;
        }
        if(CorrectShapeCount == PuzzleObjects.Count)
        {
            if(anim != null)
            {
                anim.SetBool(animationTrigger, true);
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool(animationTrigger, false);
            }
        }
    }
    [Rpc(SendTo.Server)]
    public void ScrollToNextObjectRPC(bool Add, int ShapeID)
    {
        ScrollClientSideRPC(Add,ShapeID);
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            for (int j = 0; j < ShapesToScroll[ShapeID].PuzzleObjects.Count; j++)
            {
                ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(false);
            }
        }
        UpdateShape = true;
        if(Add)
        {
            if (ShapesToScroll[ShapeID].ObjectSpawned < ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                ShapesToScroll[ShapeID].ObjectSpawned++;
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
            else if (ShapesToScroll[ShapeID].ObjectSpawned >= ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned = 1;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
        }
        else
        {
            if (ShapesToScroll[ShapeID].ObjectSpawned <= 1)
            {
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned = 3;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
            else if (ShapesToScroll[ShapeID].ObjectSpawned <= ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                ShapesToScroll[ShapeID].ObjectSpawned--;
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
        }
    }
    [Rpc(SendTo.NotServer)]
    void ScrollClientSideRPC(bool Add, int ShapeID)
    {
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            for (int j = 0; j < ShapesToScroll[ShapeID].PuzzleObjects.Count; j++)
            {
                ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(false);
            }
        }
        UpdateShape = true;
        if (Add)
        {
            if (ShapesToScroll[ShapeID].ObjectSpawned < ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                ShapesToScroll[ShapeID].ObjectSpawned++;
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
            else if (ShapesToScroll[ShapeID].ObjectSpawned >= ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned = 1;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
        }
        else
        {
            if (ShapesToScroll[ShapeID].ObjectSpawned <= 1)
            {
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned = 3;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
            else if (ShapesToScroll[ShapeID].ObjectSpawned <= ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                ShapesToScroll[ShapeID].ObjectSpawned--;
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
        }
    }
    [Rpc(SendTo.Server)]
    public void SpawnPuzzleServerRPC()
    {
        RequiredText.text = string.Empty;
        int[] objectsSpawned = new int[3];
        for (int i = 0; i < PuzzleObjects.Count; i++)
        {
                RandomRangeRPC(1, PuzzleObjects.Count+1);
            PuzzleObjects[i] = RandomizedNumber;
        }
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
                RandomRangeRPC(1, ShapesToScroll[i].PuzzleObjects.Count+1);
            int ObjectToSpawn = RandomizedNumber;
            for (int j = 0; j < ObjectToSpawn; j++)
            {
                ShapesToScroll[i].PuzzleObjects[j].SetActive(true);
            }
            ShapesToScroll[i].ObjectSpawned = ObjectToSpawn;
            if (ShapesToScroll[i].ObjectSpawned == PuzzleObjects[i])
            {
                    RandomRangeRPC(1, ShapesToScroll[i].PuzzleObjects.Count + 1);
                int ObjectToSpawnRedo = RandomizedNumber;
                for (int j = 0; j < PuzzleObjects.Count; j++)
                {
                    ShapesToScroll[i].PuzzleObjects[j].SetActive(false);
                }
                for (int j = 0; j < ObjectToSpawnRedo; j++)
                {
                    ShapesToScroll[i].PuzzleObjects[j].SetActive(true);
                }
                ShapesToScroll[i].ObjectSpawned = ObjectToSpawnRedo;
            }
        }
        int[] puzzleobj = new int[3];
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            puzzleobj[i] = PuzzleObjects[i];
            objectsSpawned[i] = ShapesToScroll[i].ObjectSpawned;
            //Debug.Log(PuzzleObjects[i] + " " + ShapesToScroll[i].ObjectSpawned);
        }

        SpawnPuzzleClientRPC(puzzleobj, objectsSpawned);
        int object1 = PuzzleObjects[0];
        int object2 = PuzzleObjects[1];
        int object3 = PuzzleObjects[2];
        RequiredText.text = object1.ToString() + " " + ShapesToScroll[0].PuzzleObjects[0].name + Environment.NewLine + object2.ToString() + " " + ShapesToScroll[1].PuzzleObjects[0].name + Environment.NewLine + object3.ToString() + " " + ShapesToScroll[2].PuzzleObjects[0].name;
    }
    int RandomizedNumber;
    [Rpc(SendTo.Server)]
    public void RandomRangeRPC(int min, int max)
    {
        if(!IsServer) return;
        RandomizedNumber = Random.Range(min, max);
        //RandomNumberRPC(RandomizedNumber);
    }
    [Rpc(SendTo.NotServer)]
    public void SpawnPuzzleClientRPC(int[] RandomNumber, int[] NumberToSpawn)
    {
        RequiredText.text = string.Empty;
        for (int i = 0; i < PuzzleObjects.Count; i++)
        {
            PuzzleObjects[i] = RandomNumber[i];
        }
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            int ObjectToSpawn = NumberToSpawn[i];
            for (int j = 0; j < ObjectToSpawn; j++)
            {
                ShapesToScroll[i].PuzzleObjects[j].SetActive(true);
            }
            ShapesToScroll[i].ObjectSpawned = ObjectToSpawn;
        }
        int object1 = PuzzleObjects[0];
        int object2 = PuzzleObjects[1];
        int object3 = PuzzleObjects[2];
        RequiredText.text = object1.ToString() + " " + ShapesToScroll[0].PuzzleObjects[0].name + Environment.NewLine + object2.ToString() + " " + ShapesToScroll[1].PuzzleObjects[0].name + Environment.NewLine + object3.ToString() + " " + ShapesToScroll[2].PuzzleObjects[0].name;
    }
}
[Serializable]
public class ShapesToRandomize
{
    public int ObjectSpawned;
    public List<GameObject> PuzzleObjects;
}
