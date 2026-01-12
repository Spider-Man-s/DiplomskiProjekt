using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCamera : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;


    [Header("Movement")]
    public float moveSpeed = 4f;
    public float mouseSensitivity = 0.15f;

    [Header("Pitch Limits")]
    public float minPitch = -80f;
    public float maxPitch = 80f;

    float yaw;
    float pitch;
    float fixedHeight;

    bool uiMode = true;

    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
    }

    void Start()
    {
        fixedHeight = transform.position.y;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        EnterUIMode();
    }

    void Update()
    {
        HandleUIToggle();

        if (!uiMode)
        {
            HandleLook();
            HandleMovement();
        }

        LockHeight();
    }

    void HandleUIToggle()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            uiMode = !uiMode;

            if (uiMode)
                EnterUIMode();
            else
                EnterFPSMode();
        }
    }

    void EnterUIMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void EnterFPSMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void HandleLook()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();

        yaw += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleMovement()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>();

        float speed = moveSpeed;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 displacement =
            (forward * move.y + right * move.x) * speed * Time.deltaTime;

        transform.position += displacement;
    }

    void LockHeight()
    {
        Vector3 pos = transform.position;
        pos.y = fixedHeight;
        transform.position = pos;
    }
}
