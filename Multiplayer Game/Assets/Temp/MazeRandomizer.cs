using System.Collections.Generic;
using UnityEngine;

public class MazeRandomizer : MonoBehaviour
{
    public List<GameObject> MazePrefabs;
    GameObject MazeObject;
    public Transform MazeSpawnLocation;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public bool ScrollPuzzle;
    public bool UpdateShape = false;
    public void SpawnRandomMaze()
    {
        UpdateShape = true;
        for (int i = 0; i < MazePrefabs.Count; i++)
        {
            int ObjectToSpawn = Random.Range(0, MazePrefabs.Count);
            GameObject PuzzleObject = Instantiate(MazePrefabs[ObjectToSpawn], MazeSpawnLocation.position, Quaternion.identity, MazeSpawnLocation);
            MazeObject = PuzzleObject;
        }
    }
    public void ResetMaze()
    {
        Destroy(MazeObject);
        SpawnRandomMaze();
    }
}
