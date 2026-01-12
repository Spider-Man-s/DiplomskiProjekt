using UnityEngine;
using TMPro;
public class ARResultsUI : MonoBehaviour
{
    [SerializeField] TMP_Text resultsText;
    [SerializeField] GameObject interruptionWarning;

    void OnEnable()
    {
        resultsText.text =
            $"Fires: {SimulationResults.ExtinguishedFires} / {SimulationResults.TotalFires}\n" +
            $"Time: {Format(SimulationResults.DurationSeconds)}";

        interruptionWarning.SetActive(
             SimulationResults.EndReason == "Disconnected"
        );
    }

    string Format(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        return $"{m:00}:{s:00}";
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
