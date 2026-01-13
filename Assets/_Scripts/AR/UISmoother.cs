using UnityEngine;

public class UISmoother : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 localOffset = new Vector3(0f, -0.1f, 0.5f);

    [Range(0.01f, 1f)]
    public float positionSmoothTime = 0.15f;

    [Range(0.01f, 1f)]
    public float rotationSmoothTime = 0.15f;

    private Vector3 positionVelocity;

    void LateUpdate()
    {
        Vector3 targetPosition =
            cameraTransform.position +
            cameraTransform.rotation * localOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref positionVelocity,
            positionSmoothTime
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            cameraTransform.rotation,
            Time.deltaTime / rotationSmoothTime
        );
    }
}
