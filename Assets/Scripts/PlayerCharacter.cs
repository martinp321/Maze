using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour
{
    private int _health;

    // Use this for initialization
    void Start()
    {
        _health = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hurt(int damange)
    {
        _health -= damange;
        Debug.Log("Health: " + _health);
    }
}
