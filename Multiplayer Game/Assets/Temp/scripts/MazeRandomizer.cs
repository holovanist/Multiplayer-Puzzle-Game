using System.Collections.Generic;
using UnityEngine;

public class MazeRandomizer : MonoBehaviour
{
    [SerializeField] List<GameObject> mazePrefabs;
    [SerializeField] Transform mazeSpawnLocation;
    [Header("Testing")]
    [SerializeField] bool spawnMaze = false;
    GameObject spawnedMazeObject;
    public int ObjectToSpawn { get; set; }

    private void FixedUpdate()
    {
        if (spawnMaze)
        {
            spawnMaze = false;
            SpawnRandomMaze();
        }
    }
    public void SpawnRandomMaze()
    {
        ObjectToSpawn = Random.Range(0, mazePrefabs.Count);
        GameObject PuzzleObject = Instantiate(mazePrefabs[ObjectToSpawn], mazeSpawnLocation.position, Quaternion.identity, mazeSpawnLocation);
        spawnedMazeObject = PuzzleObject;
    }
    public void ResetMaze()
    {
        Destroy(spawnedMazeObject);
        SpawnRandomMaze();
    }
}
