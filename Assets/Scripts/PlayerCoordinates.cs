using UnityEngine;
using TMPro;
public class PlayerCoordinates : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text debugText;

    [Header("Formatting")]
    [SerializeField]
    private int decimalPlaces = 2;

    void Update()
    {
        Vector3 pos = transform.position;
        float yRotation = transform.eulerAngles.y;

        string format = $"F{decimalPlaces}";

        debugText.text =
            $"Player Position\n" +
            $"X: {pos.x.ToString(format)}\n" +
            $"Z: {pos.z.ToString(format)}\n\n" +
            $"Rotation Y: {yRotation.ToString(format)}°";
    }
}
