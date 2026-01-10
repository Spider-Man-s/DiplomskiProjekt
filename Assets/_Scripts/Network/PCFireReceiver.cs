using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
public class PCFireReceiver : MonoBehaviour, IOnEventCallback
{
    const byte EVENT_FIRE_EXTINGUISHED = 2;

    [SerializeField] ActivateFire activateFire;
    [SerializeField] OverviewManager overview;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != EVENT_FIRE_EXTINGUISHED) return;

        int fireId = (int)photonEvent.CustomData;

        activateFire.DeActivateFlame(fireId);
        overview.OnFireExtinguished(fireId);

        Debug.Log($"[PC] Fire {fireId} extinguished");
    }
}
