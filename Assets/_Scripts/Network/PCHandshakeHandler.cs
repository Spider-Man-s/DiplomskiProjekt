using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PCHandshakeHandler : MonoBehaviour, IOnEventCallback
{
    const byte HANDSHAKE_EVENT = 20;

    [SerializeField] HandshakeController controller;

    void OnEnable()
    {
        Debug.Log("[PCHandshakeHandler] Enabled, registering callback");
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        Debug.Log("[PCHandshakeHandler] Disabled, unregistering callback");
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log($"[PCHandshakeHandler] Event received: Code={photonEvent.Code}");

        if (photonEvent.Code != HANDSHAKE_EVENT)
        {
            Debug.Log("[PCHandshakeHandler] Event ignored (not handshake)");
            return;
        }

        if (!(photonEvent.CustomData is object[] data))
        {
            Debug.LogError("[PCHandshakeHandler] Handshake data malformed!");
            return;
        }

        bool atStart = (bool)data[0];
        bool ready = (bool)data[1];

        Debug.Log($"[PCHandshakeHandler] Handshake data → AtStart={atStart}, Ready={ready}");

        controller.SetARAtStartPosition(atStart);

        if (ready)
            controller.ConfirmARReady();
    }
}
