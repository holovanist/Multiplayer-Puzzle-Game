using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapePuzzle : MonoBehaviour
{
    public List<RandomShapes> ShapesToScroll;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public List<int> PuzzleObjects;
    int CorrectShapeCount; 
    public bool ScrollPuzzle;
    public bool UpdateShape = false;
    ShapeRandomizer Randomizer;
    Animator anim;
    public string animationTrigger1;
    public string animationTrigger2;
    public string animationTrigger3;
    public string animationTrigger4;
    private void Start()
    {
        Randomizer = GetComponent<ShapeRandomizer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (SpawnPuzzleObjects)
        {
            SpawnPuzzle();
            SpawnPuzzleObjects = false;
        }
        if(Randomizer.UpdateShape)
        {
            CorrectShapeCount = 0;
            for (int i = 0;i < PuzzleObjects.Count;i++)
            {
                if (Randomizer.PuzzleObjects[i] == PuzzleObjects[i])
                {
                    CorrectShapeCount++;
                }
            }
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
    public void SpawnPuzzle()
    {
        PuzzleObjects.Clear();
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            int ObjectToSpawn = Random.Range(0, ShapesToScroll[i].PuzzleObjects.Count);
            //enable / disable objects
            ShapesToScroll[i].ObjectSpawned = ObjectToSpawn;
        }
    }
    public void SpawnNextObject(int ObjectNextSpawning)
    {
        PuzzleObjects.Clear();
        UpdateShape = true;
        for (int i = 0; i < ShapesToScroll.Count; ++i)
        {
            if (ShapesToScroll[ObjectNextSpawning].ObjectSpawned + 1 < ShapesToScroll[ObjectNextSpawning].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[ObjectNextSpawning].ObjectSpawned++;
            }
            else if (ShapesToScroll[ObjectNextSpawning].ObjectSpawned + 1 >= ShapesToScroll[i].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[ObjectNextSpawning].ObjectSpawned = 0;
            }

            PuzzleObjects[ObjectNextSpawning] = ShapesToScroll  [ObjectNextSpawning].ObjectSpawned;
        }
    }
}
    [Serializable]
    public class ShapesToRandomize
    {
        public int ObjectSpawned;
        public List<GameObject> PuzzleObjects;
    }
