using UnityEngine;

public class FireZone : MonoBehaviour
{
    public int fireId;

    [SerializeField] Fire[] fireVisuals;

    bool extinguished;

    void Awake()
    {
        ResetZone();
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);

        if (value)
            extinguished = false;
    }

    public void ApplyExtinguish(Vector3 extinguisherPos, float deltaTime)
    {
        if (extinguished) return;

        bool allOut = true;

        foreach (var fire in fireVisuals)
        {
            if (!fire.gameObject.activeInHierarchy) continue;

            if (fire.IsInRange(extinguisherPos))
            {
                fire.ApplyExtinguish(deltaTime);
            }

            if (fire.gameObject.activeInHierarchy)
                allOut = false;
        }

        if (allOut)
        {
            extinguished = true;
            gameObject.SetActive(false);
            ARFireSender.SendFireExtinguished(fireId);
        }
    }
    public void ResetZone()
    {
        extinguished = false;
        gameObject.SetActive(false);
    }
}
