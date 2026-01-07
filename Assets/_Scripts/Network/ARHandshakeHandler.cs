using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class ARHandshakeHandler : MonoBehaviourPun
{
    const byte HANDSHAKE_EVENT = 20;

    [Header("References")]
    [SerializeField] StartCircleDetector startDetector;
    [SerializeField] Button readyButton;

    bool ready;

    bool lastSentAtStart;
    bool lastSentReady;
    bool hasSentOnce;

    void Awake()
    {
        if (readyButton != null)
            readyButton.onClick.AddListener(OnReadyPressed);
    }

    void Update()
    {
        if (!PhotonNetwork.IsConnected) return;

        UpdateReadyButtonState();

        if (!PhotonNetwork.InRoom) return;
        if (!photonView.IsMine) return;
        if (startDetector == null) return;

        if (!hasSentOnce ||
            startDetector.IsAtStart != lastSentAtStart ||
            ready != lastSentReady)
        {
            SendHandshake();
        }
    }

    void UpdateReadyButtonState()
    {
        if (readyButton == null) return;

        bool canInteract =
            PhotonNetwork.IsConnected &&
            PhotonNetwork.InRoom &&
            startDetector != null &&
            startDetector.IsAtStart &&
            !ready;

        readyButton.interactable = canInteract;
    }


    void SendHandshake()
    {
        object[] data =
        {
            startDetector.IsAtStart,
            ready
        };

        PhotonNetwork.RaiseEvent(
            HANDSHAKE_EVENT,
            data,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            new SendOptions { Reliability = true }
        );

        lastSentAtStart = startDetector.IsAtStart;
        lastSentReady = ready;
        hasSentOnce = true;

        Debug.Log($"[AR] Handshake sent | AtStart={lastSentAtStart} Ready={lastSentReady}");
        Debug.Log($"[ARHandshake] SEND → AtStart={data[0]}, Ready={data[1]} Room={PhotonNetwork.CurrentRoom?.Name}");

    }

    public void OnReadyPressed()
    {
        if (ready) return;
        ready = true;
        SendHandshake();
        readyButton.gameObject.SetActive(false);
    }
}
