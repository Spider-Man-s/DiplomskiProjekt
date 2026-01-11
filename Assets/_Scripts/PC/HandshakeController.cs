using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HandshakeController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private TMP_Text waitingStatusText;

    [Header("Scene Flow")]
    [SerializeField] private string overviewSceneName = "SimulationOverview";

    SimulationPhase lastPhase;

    void Awake()
    {
        if (waitingPanel != null)
            waitingPanel.SetActive(false);
    }

    void Update()
    {
        if (SimulationSession.Instance == null)
            return;

        var sim = SimulationSession.Instance;

        UpdateWaitingUI(sim);

        if (sim.Phase != lastPhase)
        {
            lastPhase = sim.Phase;

            if (sim.Phase == SimulationPhase.Running)
            {
                Debug.Log("[HandshakeController] Handshake complete → loading overview scene");
                SceneManager.LoadScene(overviewSceneName);
            }
        }
    }

    void UpdateWaitingUI(SimulationSession sim)
    {
        if (sim.Phase != SimulationPhase.WaitingForAR)
        {
            waitingStatusText.text = "";
            return;
        }

        waitingStatusText.text =
            "<b>Čekanje AR korisnika...</b>\n\n" +
            $"• Povezan: {(sim.ARConnected ? "<color=green>DA</color>" : "<color=red>NE</color>")}\n" +
            $"• Na start poziciji: {(sim.IsARAtStart ? "<color=green>DA</color>" : "<color=red>NE</color>")}\n" +
            $"• Potvrda spremnosti: {(sim.IsARReady ? "<color=green>DA</color>" : "<color=red>NE</color>")}";
    }

    public void BeginHandshake()
    {
        waitingPanel.SetActive(true);
        SimulationSession.Instance.BeginHandshake(GameState.SelectedHouseIndex);
    }

    public void CancelHandshake()
    {
        waitingPanel.SetActive(false);
    }
}
