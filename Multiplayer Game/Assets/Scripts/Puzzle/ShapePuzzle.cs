using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapePuzzle : MonoBehaviour
{
    public List<ShapesToRandomize> ShapesToScroll;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public List<int> PuzzleObjects;
    int CorrectShapeCount;
    bool UpdateShape;
    Animator anim;
    public string animationTrigger1;
    public string animationTrigger2;
    public string animationTrigger3;
    public string animationTrigger4;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (SpawnPuzzleObjects)
        {
            SpawnPuzzle();
            SpawnPuzzleObjects = false;
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
                anim.SetBool(animationTrigger1, true);
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool(animationTrigger1, false);
            }
        }
    }
    public void ScrollToNextObject()
    {
        UpdateShape = true;
        for (int i = 0; i < ShapesToScroll.Count; ++i)
        {
            if (ShapesToScroll[i].ObjectSpawned + 1 < ShapesToScroll[i].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[i].ObjectSpawned++;
            }
            else if (ShapesToScroll[i].ObjectSpawned + 1 >= ShapesToScroll[i].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[i].ObjectSpawned = 0;
            }

            PuzzleObjects[i] = ShapesToScroll[i].ObjectSpawned;
        }
    }
    public void SpawnPuzzle()
    {
        for (int i = 0; i < PuzzleObjects.Count; i++)
        {
            PuzzleObjects[i] = Random.Range(1, PuzzleObjects.Count+1);
        }
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            int ObjectToSpawn = Random.Range(1, ShapesToScroll[i].PuzzleObjects.Count+1);
            for (int j = 0; j < ObjectToSpawn; j++)
            {
                ShapesToScroll[i].PuzzleObjects[j].SetActive(true);
            }
            ShapesToScroll[i].ObjectSpawned = ObjectToSpawn;
        }
    }
}
[Serializable]
public class ShapesToRandomize
{
    public int ObjectSpawned;
    public List<GameObject> PuzzleObjects;
}
