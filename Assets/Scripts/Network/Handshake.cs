using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandshakeRPC : MonoBehaviourPunCallbacks
{
    bool helloSent = false;

    [Header("Scene Names (set in Inspector or via NetworkBootstrap)")]
    [SerializeField] private string pcSceneName = "PCScene";
    [SerializeField] private string arSceneName = "ARScene";

    void Update()
    {
        if (!PhotonNetwork.InRoom) return;

        // Only PC scene sends the "hello" once both players are present
        if (SceneManager.GetActiveScene().name == pcSceneName
            && PhotonNetwork.CurrentRoom.PlayerCount >= 2
            && !helloSent)
        {
            helloSent = true;
            Debug.Log($"[{pcSceneName}] Sending: hello");
            photonView.RPC(nameof(RPC_Message), RpcTarget.Others, "hello");
        }
    }

    [PunRPC]
    void RPC_Message(string msg, PhotonMessageInfo info)
    {
        string scene = SceneManager.GetActiveScene().name;
        Debug.Log($"[{scene}] Received '{msg}' from {info.Sender}");

        // AR scene replies to hello
        if (scene == arSceneName && msg == "hello")
        {
            Debug.Log($"[{arSceneName}] Replying: world");
            photonView.RPC(nameof(RPC_Message), RpcTarget.Others, "world");
        }

        // PC scene receives world
        if (scene == pcSceneName && msg == "world")
        {
            Debug.Log($"[{pcSceneName}] Handshake complete!");
        }
    }
}
