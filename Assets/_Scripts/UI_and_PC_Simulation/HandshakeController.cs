using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class HandshakeController : MonoBehaviourPun
{
    [Header("UI")]
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private TMP_Text waitingStatusText;

    [Header("Scene Flow")]
    [SerializeField] private string overviewSceneName = "SimulationOverview";
    const byte HANDSHAKE_DONE_EVENT_H1 = 31;
    const byte HANDSHAKE_DONE_EVENT_H2 = 32;


    // Handshake state
    bool isWaiting;
    bool arConnected;
    bool arAtStart;
    bool arReady;

    void Awake()
    {
        if (waitingPanel != null)
            waitingPanel.SetActive(false);

        UpdateWaitingUI();
    }


    void Update()
    {
        bool connected =
            PhotonNetwork.InRoom &&
            PhotonNetwork.CurrentRoom != null &&
            PhotonNetwork.CurrentRoom.PlayerCount >= 2;

        if (connected != arConnected)
        {
            arConnected = connected;
            Debug.Log($"[HandshakeController] AR Connected = {arConnected}");
            UpdateWaitingUI();
            TryProceed();
        }
    }

    // Called by UI button: "Start Simulation"
    public void BeginHandshake()
    {
        isWaiting = true;
        arAtStart = false;
        arReady = false;

        if (waitingPanel != null)
        {
            waitingPanel.SetActive(true);
            waitingPanel.transform.SetAsLastSibling();
        }

        UpdateWaitingUI();
    }

    // Called by UI button: "Cancel"
    public void CancelHandshake()
    {
        isWaiting = false;
        arAtStart = false;
        arReady = false;

        if (waitingPanel != null)
            waitingPanel.SetActive(false);

        UpdateWaitingUI();
    }

    // ===== These are called ONLY by networking layer =====

    public void SetARAtStartPosition(bool atStart)
    {
        Debug.Log($"[HandshakeController] SetARAtStartPosition({atStart})");
        arAtStart = atStart;
        UpdateWaitingUI();
        TryProceed();
    }

    public void ConfirmARReady()
    {
        Debug.Log("[HandshakeController] ConfirmARReady()");
        arReady = true;
        UpdateWaitingUI();
        TryProceed();
    }

    // ===== Internal logic =====

    void TryProceed()
    {
        if (!isWaiting) return;

        if (arConnected && arAtStart && arReady)
        {
            FindObjectOfType<ActivateFire>()?.SendSelectedFiresToAR();
            if (GameState.SelectedHouseIndex == 0)
            {
                PhotonNetwork.RaiseEvent(
                        HANDSHAKE_DONE_EVENT_H1,
                        null,
                        new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                        SendOptions.SendReliable
                    );

                Debug.Log("[HandshakeController] HANDSHAKE_DONE_EVENT_H1 sent");
            }
            else if (GameState.SelectedHouseIndex == 1)
            {
                PhotonNetwork.RaiseEvent(
                        HANDSHAKE_DONE_EVENT_H2,
                        null,
                        new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                        SendOptions.SendReliable
                    );

                Debug.Log("[HandshakeController] HANDSHAKE_DONE_EVENT_H2 sent");
            }

            Debug.Log("[HandshakeController] HANDSHAKE_DONE_EVENT sent");

            SceneManager.LoadScene(overviewSceneName);
        }
    }

    void UpdateWaitingUI()
    {
        if (waitingStatusText == null) return;

        if (!isWaiting)
        {
            waitingStatusText.text = "";
            return;
        }

        waitingStatusText.text =
            "<b>Čekanje AR korisnika...</b>\n\n" +
            $"• Povezan: {(arConnected ? "<color=green>DA</color>" : "<color=red>NE</color>")}\n" +
            $"• Na start poziciji: {(arAtStart ? "<color=green>DA</color>" : "<color=red>NE</color>")}\n" +
            $"• Potvrda spremnosti: {(arReady ? "<color=green>DA</color>" : "<color=red>NE</color>")}";
    }
}
