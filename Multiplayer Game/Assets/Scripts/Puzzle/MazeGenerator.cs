using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCellPrefab;
    [SerializeField] GameObject mazeCenter;
    [SerializeField] int mazeWidth;
    [SerializeField] int mazeDepth;
    private MazeCell[,] mazeGrid;
    void Start()
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

        GenerateMaze(null, mazeGrid[0, 0]);
    }
    void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        //clears top of cell making inside visible
        currentCell.Visit();

        //clears conecting walls 
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            //calls generate maze again if next cell is available
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }
    MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }
    IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        //Checks for nearby unvisited cells based off current cell position
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;
        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if (cellToRight.visited == false)
            {
                yield return cellToRight;
            }
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.visited == false)
            {
                yield return cellToLeft;
            }
        }
        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];
            if (cellToFront.visited == false)
            {
                yield return cellToFront;
            }
        }
        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if (cellToBack.visited == false)
            {
                yield return cellToBack;
            }
        }
    }
    void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        //only triggers on first cell
        if (previousCell == null)
        {
            return;
        }
        Vector3 prevCellPosition = previousCell.transform.position;
        Vector3 curCellPosition = currentCell.transform.position;

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
        
    }
}