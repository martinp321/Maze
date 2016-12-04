using UnityEngine;
using System.Collections;

public class FPSInp : MonoBehaviour {

    public float speed = 6.0f;
    private CharacterController _charController;

	// Use this for initialization
	void Start () {
        _charController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX,0,deltaZ);

        movement = Vector3.ClampMagnitude(movement, speed);

        movement *= dt;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);

	}
}
