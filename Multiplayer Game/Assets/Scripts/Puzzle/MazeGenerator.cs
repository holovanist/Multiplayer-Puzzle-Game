using System.Collections;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    /*[SerializeField] MazeCell mazeCellPrefab;
    [SerializeField] GameObject mazeCenter;
    [SerializeField] int mazeWidth;
    [SerializeField] int mazeDepth;
    private MazeCell[,] mazeGrid;
    IEnumerator Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];
        Vector3 spawnPosition;
        //checks if maze center object is assigned
        if (mazeCenter != null)
        {
            spawnPosition = mazeCenter.transform.position;
        }else
        {
            spawnPosition = Vector3.zero;
            Debug.LogWarning("Maze spawn position not set!");
        }
        //fills maze grid with cells
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x + spawnPosition.x, 0 + spawnPosition.y, z + spawnPosition.z),Quaternion.identity, mazeCenter.transform);
            }
        }
    }
    IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        //clears top of cell making inside visible
        currentCell.Visit();
        //clears conecting walls 
        ClearWalls(previousCell, currentCell);
    }
    MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {

    }
    IEnumerable<MazeCell> GetNextUnvisitedCell(MazeCell currentCell)
    {

    }
    void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        Vector3 prevCellPosition = previousCell.transform.position;
        Vector3 curCellPosition = currentCell.transform.position;
        //only triggers on first cell
        if (previousCell == null)
        {
            return;
        }
        //logic for clearing cell walls based of direction moved
        if (prevCellPosition.x < curCellPosition.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        if (prevCellPosition.x > curCellPosition.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        if (prevCellPosition.z < curCellPosition.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        if (prevCellPosition.z > curCellPosition.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
    void Update()
    {
        
    }*/
}