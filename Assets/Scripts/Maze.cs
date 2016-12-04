using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour
{

    public IntVector2 mazeSize;
    public MazeCell cellPrefab;
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
        IntVector2 coordinates = RandomCoordinates;
        while (ContainsCoordinates(coordinates) &&
            GetCell(coordinates) == null)
        {
            yield return delay;
            CreateCell(coordinates);
            coordinates += MazeDirections.RandomValue.ToIntVector2();
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

    private void CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab);
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(coordinates.x - mazeSize.x * .5f + .5f, 0f, coordinates.z - mazeSize.z * .5f + .5f);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
