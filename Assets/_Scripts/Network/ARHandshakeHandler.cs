using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class ARHandshakeHandler : MonoBehaviour, IOnEventCallback
{
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

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SimEvents.SIMULATION_END ||
            photonEvent.Code == SimEvents.SIMULATION_RESET)
        {
            ResetHandshake();
        }
    }

    void ResetHandshake()
    {
        ready = false;
        hasSentOnce = false;
        lastSentAtStart = false;
        lastSentReady = false;

        if (readyButton != null)
            readyButton.gameObject.SetActive(true);

        Debug.Log("[ARHandshake] Reset");
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        if (startDetector == null) return;

        UpdateReadyButtonState();

        bool atStart = startDetector.IsAtStart;

        if (!hasSentOnce ||
            atStart != lastSentAtStart ||
            ready != lastSentReady)
        {
            SendStatus(atStart, ready);
        }
    }

    void UpdateReadyButtonState()
    {
        if (readyButton == null) return;

        readyButton.interactable =
            PhotonNetwork.InRoom &&
            startDetector != null &&
            startDetector.IsAtStart &&
            !ready;
    }

    void SendStatus(bool atStart, bool ready)
    {
        PhotonNetwork.RaiseEvent(
            SimEvents.AR_STATUS_UPDATE,
            new object[] { atStart, ready },
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendUnreliable
        );

        lastSentAtStart = atStart;
        lastSentReady = ready;
        hasSentOnce = true;

        Debug.Log($"[ARHandshake] STATUS → AtStart={atStart}, Ready={ready}");
    }

    public void OnReadyPressed()
    {
        if (ready) return;

        ready = true;
        SendStatus(startDetector.IsAtStart, ready);

        if (readyButton != null)
            readyButton.gameObject.SetActive(false);
    }
}
