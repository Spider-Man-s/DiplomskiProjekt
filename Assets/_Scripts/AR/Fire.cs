using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Extinguish Settings")]
    public float totalExtinguishTime = 3f;
    public float deactivateScaleThreshold = 0.01f;
    public float extinguishRadius = 1.5f;

    private float extinguishProgress = 0f;
    private Vector3 initialScale;
    private bool isExtinguished = false;

    void Awake()
    {
        initialScale = transform.localScale;
    }
    public bool IsInRange(Vector3 extinguisherPosition)
    {
        return Vector3.Distance(extinguisherPosition, transform.position)
               <= extinguishRadius;
    }
    public void ApplyExtinguish(float deltaTime)
    {
        if (isExtinguished) return;

        extinguishProgress += deltaTime;
        extinguishProgress = Mathf.Clamp(extinguishProgress, 0f, totalExtinguishTime);

        float remainingRatio = 1f - (extinguishProgress / totalExtinguishTime);
        transform.localScale = initialScale * remainingRatio;

        if (remainingRatio <= deactivateScaleThreshold)
        {
            ExtinguishCompletely();
        }
    }

    private void ExtinguishCompletely()
    {
        isExtinguished = true;
        gameObject.SetActive(false);
    }
}
