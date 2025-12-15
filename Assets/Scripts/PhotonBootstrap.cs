using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviourPunCallbacks
{
    public static NetworkBootstrap Instance;

    [SerializeField] string roomName = "GoriGoraGoriBorovina";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(roomName,
            new RoomOptions { MaxPlayers = 2 },
            TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}, Players: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
}