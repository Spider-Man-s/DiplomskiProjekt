using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;     // kuća oko koje orbitiraš
    public float distance = 5f;  // udaljenost od kuće
    public float xSpeed = 120f;  // brzina rotacije po X 
    public float ySpeed = 120f;  // brzina rotacije po Y

    private float yaw = 0f;
    private float pitch = 20f;   // mali nagib prema dolje 
    public float zoomSpeed = 5f;
    public float minDistance = 2f;
    public float maxDistance = 20f;

    void LateUpdate()
    {
        if (!target) return;
        // ZOOM - scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;  
            pitch -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            // ograniči da se kamera ne prevrne
            pitch = Mathf.Clamp(pitch, -80f, 80f);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        transform.position = position;
        transform.LookAt(target);
    }
}
