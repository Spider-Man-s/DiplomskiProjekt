using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ARFireReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte EVENT_FIRE_ACTIVATED = 1;

    private void OnEnable()  => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != EVENT_FIRE_ACTIVATED) return;

        // Mi šaljemo int flameID
        int flameID = (int)photonEvent.CustomData;

        Debug.Log($"[AR] upalila se vatra [{flameID}]");

        // Ovdje dalje radiš što treba na AR strani:
        // - upališ odgovarajući AR efekt
        // - pokažeš UI poruku itd.
    }
}