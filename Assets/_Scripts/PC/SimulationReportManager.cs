using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class SimulationReportManager : MonoBehaviour
{
    [SerializeField] private TMP_Text firesText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        // pročitaj rezultate
        int total = SimulationResults.TotalFires;
        int ext = SimulationResults.ExtinguishedFires;
        float duration = SimulationResults.DurationSeconds;

        if (firesText != null)
        {
            firesText.text = $"Fires extinguished: {ext} / {total}";
        }

        if (timeText != null)
        {
            TimeSpan t = TimeSpan.FromSeconds(duration);
            timeText.text = $"Time: {t.Minutes:00}:{t.Seconds:00}";
        }
    }

    public void OnCloseButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
