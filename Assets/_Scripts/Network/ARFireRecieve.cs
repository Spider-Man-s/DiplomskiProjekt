using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ARFireReceiver : MonoBehaviourPunCallbacks//, IOnEventCallback
{/*
    private const byte EVENT_FIRE_ACTIVATED = 1;

    [SerializeField] FireZone[] fireZones;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != EVENT_FIRE_ACTIVATED) return;

        int fireId = (int)photonEvent.CustomData;

        foreach (var zone in fireZones)
        {
            if (zone.fireId == fireId)
            {
                zone.SetActive(true);
                Debug.Log($"[AR] FireZone {fireId} activated");
                return;
            }
        }
    }
    */
}