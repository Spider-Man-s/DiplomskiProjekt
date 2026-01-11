using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public enum SimulationPhase
{
    Idle,
    WaitingForAR,
    Running,
    Resetting
}

public enum ARState
{
    Disconnected,
    Connected,
    AtStart,
    Ready
}

public class SimulationSession :
    MonoBehaviourPunCallbacks,
    IOnEventCallback
{
    public static SimulationSession Instance;

    public SimulationPhase Phase { get; private set; } = SimulationPhase.Idle;

    // ===== Cached network state =====
    bool arAtStart;
    bool arReady;
    bool pcWaiting;
    int selectedHouseIndex = -1;

    public bool ARConnected =>
        PhotonNetwork.InRoom &&
        PhotonNetwork.CurrentRoom != null &&
        PhotonNetwork.CurrentRoom.PlayerCount >= 2;

    public ARState CurrentARState
    {
        get
        {
            if (!ARConnected) return ARState.Disconnected;
            if (arReady) return ARState.Ready;
            if (arAtStart) return ARState.AtStart;
            return ARState.Connected;
        }
    }

    public bool IsARConnected => ARConnected;
    public bool IsARAtStart => arAtStart;
    public bool IsARReady => arReady;
    public bool IsWaitingForAR => Phase == SimulationPhase.WaitingForAR;
    public int SelectedHouseIndex => selectedHouseIndex;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    // ===================== EVENTS =====================

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case SimEvents.AR_STATUS_UPDATE:
                HandleARStatus(photonEvent.CustomData);
                break;

            case SimEvents.FIRE_SELECTION:
                ARSimulationState.SelectedFireIds =
                    (int[])photonEvent.CustomData;
                break;
        }
    }

    void HandleARStatus(object payload)
    {
        object[] data = (object[])payload;

        arAtStart = (bool)data[0];
        arReady = (bool)data[1];

        Debug.Log($"[SimSession] AR status → AtStart={arAtStart}, Ready={arReady}");
        TryAdvanceHandshake();
    }

    // ===================== PC ENTRY =====================

    public void BeginHandshake(int houseIndex)
    {
        pcWaiting = true;
        selectedHouseIndex = houseIndex;
        Phase = SimulationPhase.WaitingForAR;

        Debug.Log($"[SimSession] PC requested handshake | House={houseIndex}");
        TryAdvanceHandshake();
    }

    // ===================== CORE LOGIC =====================

    void TryAdvanceHandshake()
    {
        if (Phase != SimulationPhase.WaitingForAR) return;
        if (!pcWaiting) return;
        if (!ARConnected) return;
        if (!arAtStart) return;
        if (!arReady) return;
        if (selectedHouseIndex < 0) return;

        Debug.Log("[SimSession] HANDSHAKE COMPLETE");

        Phase = SimulationPhase.Running;

        var activateFire = FindObjectOfType<ActivateFire>();
        if (activateFire != null)
        {
            activateFire.SendSelectedFiresToAR();
            Debug.Log("[SimSession] Fire selection sent");
        }
        else
        {
            Debug.LogWarning("[SimSession] ActivateFire not found!");
        }

        PhotonNetwork.RaiseEvent(
            SimEvents.HANDSHAKE_DONE,
            selectedHouseIndex,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable
        );
    }

    // ===================== DISCONNECT =====================

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("[SimSession] Player disconnected");

        Phase = SimulationPhase.Resetting;

        PhotonNetwork.RaiseEvent(
            SimEvents.SIMULATION_RESET,
            null,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            SendOptions.SendReliable
        );
    }
}
