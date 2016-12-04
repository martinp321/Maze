using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour
{

    public IntVector2 mazeSize;
    public MazeCell cellPrefab;
    public MazeCellWall wallPrefab;
    public MazeCellPassage passagePrefab;
    private MazeCell[,] cells;

    public float generationStepDelay = .1f;

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[mazeSize.x, mazeSize.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIdx = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIdx];
        if (currentCell.IsFullInitialized)
        {
            activeCells.RemoveAt(currentIdx);
            return;
        }

        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else CreateWall(currentCell, neighbor, direction);
        }
        else CreateWall(currentCell, null, direction);
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        CreateEdge(cell, otherCell, direction, wallPrefab);
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        CreateEdge(cell, otherCell, direction, passagePrefab);
    }

    private void CreateEdge(MazeCell cell, MazeCell otherCell, MazeDirection direction, MazeCellEdge prefab)
    {
        MazeCellEdge edge = Instantiate(prefab);
        edge.Initialize(cell, otherCell, direction);

        if (otherCell != null)
        {
            edge = Instantiate(prefab);
            edge.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return (coordinate.x >= 0 && coordinate.z >= 0)
            && (coordinate.x < mazeSize.x && coordinate.z < mazeSize.z);
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab);
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - mazeSize.x * .5f + .5f, 0f, coordinates.z - mazeSize.z * .5f + .5f);
        return newCell;
    }

}
