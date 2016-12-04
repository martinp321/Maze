using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public PlayerCharacter playerPrefab;
    private PlayerCharacter playerInstance;

    public Maze mazePrefab;
    private Maze mazeInstance;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(BeginGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    private IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        playerInstance = Instantiate(playerPrefab) as PlayerCharacter;
        Camera.main.rect = new Rect(0f, 0f, .5f, .5f);
        yield return StartCoroutine(mazeInstance.Generate());


    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        Destroy(playerInstance.gameObject);
        BeginGame();
    }
}
