using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
public class ARResultListener : MonoBehaviour, IOnEventCallback
{
    [SerializeField] GameObject resultsPanel;
    [SerializeField] Button xCloseButton;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != SimEvents.SIMULATION_END)
            return;

        object[] data = (object[])photonEvent.CustomData;

        SimulationResults.TotalFires = (int)data[0];
        SimulationResults.ExtinguishedFires = (int)data[1];
        SimulationResults.DurationSeconds = (float)data[2];
        SimulationResults.EndReason = (string)data[3];

        ShowResultsUI();
    }

    void ShowResultsUI()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        Debug.Log("[AR] Results UI shown");
    }

    public void OnCloseResultsPressed()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        Debug.Log("[AR] Results UI closed");
    }
}
