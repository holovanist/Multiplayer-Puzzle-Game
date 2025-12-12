using System.Collections.Generic;
using UnityEngine;

public class PuzzleRandomizer : MonoBehaviour
{
    public List<GameObject> PuzzleObjects;
    List<GameObject> StaticPuzzleObjects = new();
    public List<Transform> PuzzleSpawnLocations;
    List<GameObject> SpawnedObjects = new();
    public bool temp;
    public bool temp1;
    void Start()
    {
        StaticPuzzleObjects.AddRange(PuzzleObjects);
    }
    private void Update()
    {
        if(temp)
        {
            SpawnPuzzle();
            temp = false;
        }
        if(temp1)
        {
            ResetPuzzle();
            temp1 = false;
        }
    }
    public void SpawnPuzzle()
    {
        for(int i = 0; i < PuzzleSpawnLocations.Count; i++)
        {
            int ObjectToSpawn = Random.Range(0, PuzzleObjects.Count);
            GameObject a = Instantiate(PuzzleObjects[ObjectToSpawn], PuzzleSpawnLocations[i].position, Quaternion.identity, PuzzleSpawnLocations[i]);
            SpawnedObjects.Add(a);
            PuzzleObjects.RemoveAt(ObjectToSpawn);
        }
    }
    public void ResetPuzzle()
    {
        for(int i = 0;i < SpawnedObjects.Count;i++)
        {
            Destroy(SpawnedObjects[i]);
        }
        SpawnedObjects.Clear();
        PuzzleObjects.Clear();
        PuzzleObjects.AddRange(StaticPuzzleObjects);
    }
}
