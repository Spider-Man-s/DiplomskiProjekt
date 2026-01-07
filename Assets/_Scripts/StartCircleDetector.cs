using UnityEngine;
using TMPro;
public class StartCircleDetector : MonoBehaviour
{
    [SerializeField] float radius = 1.5f;
    [SerializeField] TMP_Text debugText;

    public bool IsAtStart { get; private set; }

    void Update()
    {
        Vector2 posXZ = new Vector2(transform.position.x, transform.position.z);
        IsAtStart = posXZ.magnitude <= radius;
        if (debugText != null)
        {
            debugText.text = IsAtStart ? "At Start Position" : "Away from Start";
        }
    }
}
