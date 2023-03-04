using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float mouse_sensitivity = 100f;
    public Transform player_body;
    public Transform camera_view;

    float x_rotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        x_rotation = camera_view.localEulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseTransition.isPaused)
        {
            float mouse_x = Input.GetAxis("Mouse X") * mouse_sensitivity * Time.deltaTime;
            float mouse_y = Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;

            player_body.Rotate(Vector3.up * mouse_x);

            x_rotation -= mouse_y;
            x_rotation = Mathf.Clamp(x_rotation, -90, 90);
            camera_view.localEulerAngles = Vector3.right * x_rotation;
        }
    }
}
