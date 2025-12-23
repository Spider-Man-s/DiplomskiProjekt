using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviourPunCallbacks
{
    public static NetworkBootstrap Instance;

    [Header("Room Settings")]
    [SerializeField] string roomName = "GoriGoraGoriBorovina";

    [Header("Scene Names")]
    [SerializeField] string arSceneName = "ARScene";
    [SerializeField] string pcMapSceneName = "PCScene";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("Bootstrap Start. Connecting...");
        PhotonNetwork.AutomaticallySyncScene = false;

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        else
            Debug.Log("Already connected.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster. Joining room...");
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom: {PhotonNetwork.CurrentRoom.Name} count={PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRoomFailed {returnCode} {message}");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError($"Disconnected: {cause}");
    }
    public void LoadARScene()
    {
        PhotonNetwork.LoadLevel(arSceneName);
    }

    public void LoadPCMapScene()
    {
        PhotonNetwork.LoadLevel(pcMapSceneName);
    }
}