using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{

    public float sensitivityX = 9.0f;
    public float sensitivityY = 9.0f;

    private float _rotationX = 0f;

    public float minY = -45f;
    public float maxY = 45f;

    // Use this for initialization
    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveView(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void MoveView(float mouseX, float mouseY)
    {
        _rotationX -= mouseY * sensitivityY;
        _rotationX = Mathf.Clamp(_rotationX, minY, maxY);

        float delta = mouseX * sensitivityX;
        float rotationY = transform.localEulerAngles.y + delta;

        transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
    }
}
