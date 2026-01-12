using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationResultsUI : MonoBehaviour
{
    [Header("UI (TextMeshPro)")]
    [SerializeField] private TMP_Text resultsText;

    [Header("Warnings")]
    [SerializeField] private GameObject arDisconnectedWarning;

    private void OnEnable()
    {
        UpdateResultsUI();
    }

    private void UpdateResultsUI()
    {
        if (resultsText == null) return;

        resultsText.text =
            $"Ukupno požara: {SimulationResults.TotalFires}\n" +
            $"Ugašeno: {SimulationResults.ExtinguishedFires}\n" +
            $"Vrijeme trajanja: {FormatTime(SimulationResults.DurationSeconds)}";

        if (arDisconnectedWarning != null)
            arDisconnectedWarning.SetActive(SimulationResults.EndReason == "Disconnected");
    }

    private string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:00}:{sec:00}";
    }

    public void OnReplayButton()
    {
        GameState.ReplayRequested = true;
        SceneManager.LoadScene("SimulationMenu");
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

