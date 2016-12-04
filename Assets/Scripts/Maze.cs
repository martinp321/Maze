using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Maze : MonoBehaviour
{
    private List<MazeRoom> rooms = new List<MazeRoom>();

    public MazeRoomSettings[] roomSettings;

    public IntVector2 mazeSize;
    public MazeCell cellPrefab;
    public MazeDoor doorPrefab;

    public MazeCellWall[] wallPrefabs;

    public MazeCellPassage passagePrefab;
    private MazeCell[,] cells;

    public float generationStepDelay = .1f;

    [Range(0f, 1f)]
    public float doorProbability = .2f;

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
        MazeCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
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
            else if (currentCell.room.settingsIdx == neighbor.room.settingsIdx)
            {
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            }
            else CreateWall(currentCell, neighbor, direction);
        }
        else CreateWall(currentCell, null, direction);
    }



    private void CreatePassageInSameRoom(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeCellPassage passage = Instantiate(passagePrefab) as MazeCellPassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazeCellPassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
        if (cell.room != otherCell.room)
        {
            MazeRoom roomToAssimilate = otherCell.room;
            cell.room.Assimilate(roomToAssimilate);
            rooms.Remove(roomToAssimilate);
            Destroy(roomToAssimilate);
        }
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeCellWall wallPrefab = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        CreateEdge(cell, otherCell, direction, wallPrefab);
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeCellPassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        MazeCellPassage passage = Instantiate(prefab) as MazeCellPassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as MazeCellPassage;
        if (passage is MazeDoor)
        {
            otherCell.Initialize(CreateRoom(cell.room.settingsIdx));
        }
        else
        {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
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

    private MazeRoom CreateRoom(int indexToExclude)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingsIdx = Random.Range(0, roomSettings.Length);
        if (newRoom.settingsIdx == indexToExclude)
        {
            newRoom.settingsIdx = (newRoom.settingsIdx + 1) % roomSettings.Length;
        }
        newRoom.settings = roomSettings[newRoom.settingsIdx];
        rooms.Add(newRoom);
        return newRoom;
    }

}
