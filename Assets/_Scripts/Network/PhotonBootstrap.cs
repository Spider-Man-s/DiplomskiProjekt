using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviourPunCallbacks
{
    public static NetworkBootstrap Instance;

    [Header("Room Settings")]
    [SerializeField] private string roomName = "GoriGoraGoriBorovina";
    [SerializeField] private byte maxPlayers = 2;

    void Awake()
    {
        // Singleton protection
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("[NetworkBootstrap] Starting…");

        PhotonNetwork.AutomaticallySyncScene = false;

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("[NetworkBootstrap] Connecting to Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("[NetworkBootstrap] Already connected.");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[NetworkBootstrap] Connected to Master. Joining room…");

        PhotonNetwork.JoinOrCreateRoom(
            roomName,
            new RoomOptions { MaxPlayers = maxPlayers },
            TypedLobby.Default
        );
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"[NetworkBootstrap] Joined room '{PhotonNetwork.CurrentRoom.Name}' " +
                  $"({PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers})");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"[NetworkBootstrap] Join room failed ({returnCode}): {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"[NetworkBootstrap] Disconnected: {cause}");
    }
}
