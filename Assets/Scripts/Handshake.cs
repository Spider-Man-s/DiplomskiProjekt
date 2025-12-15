using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandshakeRPC : MonoBehaviourPunCallbacks
{
    bool helloSent = false;

    void Update()
    {
        if (!PhotonNetwork.InRoom) return;

        // SceneA šalje hello kad su oba klijenta u roomu
        if (SceneManager.GetActiveScene().name == "PCScene"
            && PhotonNetwork.CurrentRoom.PlayerCount >= 2
            && !helloSent)
        {
            helloSent = true;
            Debug.Log("[SceneA] Sending: hello");
            photonView.RPC(nameof(RPC_Message), RpcTarget.Others, "hello");
        }
    }

    [PunRPC]
    void RPC_Message(string msg, PhotonMessageInfo info)
    {
        var scene = SceneManager.GetActiveScene().name;
        Debug.Log($"[{scene}] Received '{msg}' from {info.Sender}");

        // SceneB odgovara na hello
        if (scene == "ARScene" && msg == "hello")
        {
            Debug.Log("[SceneB] Replying: world");
            photonView.RPC(nameof(RPC_Message), RpcTarget.Others, "world");
        }

        // SceneA prima world
        if (scene == "PCScene" && msg == "world")
        {
            Debug.Log("[SceneA] Handshake complete!");
        }
    }
}