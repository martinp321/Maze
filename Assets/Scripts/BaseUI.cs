using UnityEngine;
using System.Collections;

public class BaseUI : MonoBehaviour
{

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 40, 20), "TEST"))
        {
            Debug.Log("Test Button");
        }
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
