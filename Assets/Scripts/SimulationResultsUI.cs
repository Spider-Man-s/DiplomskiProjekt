using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationResultsUI : MonoBehaviour
{
    [Header("UI (TextMeshPro)")]
    [SerializeField] private TMP_Text resultsText;

    private void Start()
    {
        UpdateResultsUI();
    }

    private void UpdateResultsUI()
    {
        if (resultsText == null)
            return;

        resultsText.text =
            $"Ukupno požara: {SimulationResults.TotalFires}\n" +
            $"Ugašeno: {SimulationResults.ExtinguishedFires}\n" +
            $"Vrijeme trajanja: {FormatTime(SimulationResults.DurationSeconds)}";
    }

    private string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:00}:{sec:00}";
    }

    // -----------------------------
    // GUMB: PONOVI SIMULACIJU
    // -----------------------------
    public void OnReplayButton()
    {
        // reset rezultata (vizualno i logički čisto)
        SimulationResults.TotalFires = 0;
        SimulationResults.ExtinguishedFires = 0;
        SimulationResults.DurationSeconds = 0f;  // :contentReference[oaicite:0]{index=0}

        // označi da je replay (ali NE diramo SelectedFireIndices)
        GameState.ReplayRequested = true;

        // vrati na SimulationMenu gdje se automatski pokreće handshake
        SceneManager.LoadScene("SimulationMenu");
    }

    // -----------------------------
    // GUMB: POVRATAK NA MAIN MENU (opcionalno)
    // -----------------------------
    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
