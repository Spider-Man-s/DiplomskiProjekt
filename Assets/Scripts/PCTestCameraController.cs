using UnityEngine;

public class PCTestCameraController : MonoBehaviour
{
    [Header("Kretanje")]
    public float moveSpeed = 5f;
    public float fastMultiplier = 3f;

    [Header("Rotacija mišem")]
    public float mouseSensitivity = 2f;
    public bool invertY = false;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Start()
    {
        Vector3 euler = transform.localEulerAngles;
        rotationY = euler.y;
        rotationX = euler.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();

        // ESC za otključati kursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX += invertY ? mouseY : -mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    private void HandleMovement()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= fastMultiplier;

        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S

        Vector3 move = (transform.forward * v + transform.right * h);

        // gore/dolje (Space / LeftCtrl)
        if (Input.GetKey(KeyCode.Space))
            move += Vector3.up;
        if (Input.GetKey(KeyCode.LeftControl))
            move += Vector3.down;

        transform.position += move * speed * Time.deltaTime;
    }
}
