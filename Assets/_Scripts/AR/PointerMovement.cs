using UnityEngine;

public class PointerMovement : MonoBehaviour
{
    [Header("Float")]
    public float floatAmplitude = 0.03f;
    public float floatFrequency = 0.8f;

    [Header("Rotation")]
    public float rotationSpeed = 15f;

    private Vector3 basePosition;
    private float phaseOffset;

    void Start()
    {
        basePosition = transform.position;

        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void LateUpdate()
    {
        float yOffset =
            Mathf.Sin((Time.time * floatFrequency * Mathf.PI * 2f) + phaseOffset)
            * floatAmplitude;

        transform.position = basePosition + Vector3.up * yOffset;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
