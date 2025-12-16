using System.Collections.Generic;
using UnityEngine;

public class ShapePuzzle : MonoBehaviour
{
    public List<RandomShapes> ShapesToRandomize;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public List<int> PuzzleObjects;
    int CorrectShapeCount;
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
            for (int i = 0; i < ShapesToRandomize.Count; i++)
            {
                if (ShapesToRandomize[i].SpawnedObjects != null)
                {
                    ResetPuzzle();
                }
            }
            SpawnPuzzle();
            SpawnPuzzleObjects = false;
        }
        if (RemovePuzzle)
        {
            ResetPuzzle();
            RemovePuzzle = false;
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
        for (int i = 0; i < ShapesToRandomize.Count; i++)
        {
            if (ShapesToRandomize[i].PuzzleSpawnLocations != null)
            {
            int ObjectToSpawn = Random.Range(0, ShapesToRandomize[i].PuzzleObjects.Count);
            GameObject PuzzleObject = Instantiate(ShapesToRandomize[i].PuzzleObjects[ObjectToSpawn], ShapesToRandomize[i].PuzzleSpawnLocations.position, Quaternion.identity, ShapesToRandomize[i].PuzzleSpawnLocations);
            ShapesToRandomize[i].SpawnedObjects = PuzzleObject;
            ShapesToRandomize[i].ObjectSpawned = ObjectToSpawn;
            }
            else
            {
                int ObjectToSpawn = Random.Range(0, ShapesToRandomize[i].PuzzleObjects.Count);
                PuzzleObjects.Add(ObjectToSpawn);
            }
        }
    }
    public void ResetPuzzle()
    {
        for (int i = 0; i < ShapesToRandomize.Count; i++)
        {
            Destroy(ShapesToRandomize[i].SpawnedObjects);
        }
    }
}
