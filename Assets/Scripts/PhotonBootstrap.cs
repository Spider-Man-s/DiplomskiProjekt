using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonBootstrap : MonoBehaviourPunCallbacks
{
    [SerializeField] private string roomName = "GoriGoraGoriBorovina"; 

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = false; 
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            JoinRoom();
        }
    }

    public override void OnConnectedToMaster()
    {
        JoinRoom();
    }

    private void JoinRoom()
    {
        var roomOptions = new RoomOptions { MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Photon room: " + roomName);
    }
}