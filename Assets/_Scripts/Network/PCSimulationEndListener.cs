using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PCSimulationEndListener : MonoBehaviour, IOnEventCallback
{
    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != SimEvents.SIMULATION_END)
            return;

        Debug.Log("[PC] Simulation ended → loading report scene");

        SceneManager.LoadScene("SimulationReport");
    }
}
