using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeRandomizer : MonoBehaviour
{
    public List<RandomShapes> ShapesToRandomize;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public bool ScrollPuzzle;
    public List<int> PuzzleObjects;
    public bool UpdateShape = false;
    private void Update()
    {
        if(ScrollPuzzle)
        {
            SpawnNextObject();
            ScrollPuzzle = false;
        }
    }
    public void SpawnNextObject()
    {
        PuzzleObjects.Clear();
        UpdateShape = true;
        for (int i = 0; i < ShapesToRandomize.Count; ++i)
        {
            if(ShapesToRandomize[i].ObjectSpawned+1 < ShapesToRandomize[i].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToRandomize[i].ObjectSpawned++;
                Spawn(ObjectToSpawn, i);
            }
            else if (ShapesToRandomize[i].ObjectSpawned+1 >= ShapesToRandomize[i].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToRandomize[i].ObjectSpawned = 0;
                Spawn(ObjectToSpawn, i);
            }
            
            PuzzleObjects[i] = ShapesToRandomize[i].ObjectSpawned;
        }
    }
    private void Spawn(int ObjectToSpawn, int i)
    {
        GameObject PuzzleObject = Instantiate(ShapesToRandomize[i].PuzzleObjects[ShapesToRandomize[i].ObjectSpawned], ShapesToRandomize[i].PuzzleSpawnLocations.position, Quaternion.identity, ShapesToRandomize[i].PuzzleSpawnLocations);
        ShapesToRandomize[i].SpawnedObjects = PuzzleObject;
        PuzzleObjects.Add(ObjectToSpawn);
    }
}
[Serializable]
public class RandomShapes
{
    public int ObjectSpawned;
    public List<GameObject> PuzzleObjects;
    public Transform PuzzleSpawnLocations;
    public GameObject SpawnedObjects;
}
