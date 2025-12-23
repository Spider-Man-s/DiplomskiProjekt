using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerCoordinates : MonoBehaviourPun
{
    /*
    [Header("UI")]
    [SerializeField] TMP_Text debugText;

    [Header("Formatting")]
    [SerializeField] int decimalPlaces = 2;

    [Header("Networking")]
    [SerializeField] float sendRate = 0.1f;   // 10 times per second
    float timer;

    void Update()
    {
        Vector3 pos = transform.position;
        float yRotation = transform.eulerAngles.y;

        string format = $"F{decimalPlaces}";
        debugText.text =
            $"Player Position\n" +
            $"X: {pos.x.ToString(format)}\n" +
            $"Z: {pos.z.ToString(format)}\n\n" +
            $"Rotation Y: {yRotation.ToString(format)}°";

        SendNetworkPose(pos, yRotation);
    }

    void SendNetworkPose(Vector3 pos, float rotY)
    {
        if (!PhotonNetwork.InRoom) return;
        if (!photonView.IsMine) return;

        timer += Time.deltaTime;
        if (timer < sendRate) return;
        timer = 0;

        photonView.RPC(nameof(RPC_UpdatePoseOnOtherClients), RpcTarget.Others, pos, rotY);
    }

    [PunRPC]
    void RPC_UpdatePoseOnOtherClients(Vector3 pos, float rotY)
    {
        // This will ONLY run on PC device
        PlayerNetworkState.Position = pos;
        PlayerNetworkState.RotationY = rotY;
    }
    */

    const byte POSITION_EVENT = 10;

    [SerializeField] float sendRate = 10f;
    float timer;

    void Update()
    {
        if (!PhotonNetwork.InRoom) return;

        timer += Time.deltaTime;
        if (timer < 1f / sendRate) return;
        timer = 0f;

        Vector3 p = transform.position;
        float y = transform.eulerAngles.y;

        object[] data = { p.x, p.y, p.z, y };

        PhotonNetwork.RaiseEvent(
            POSITION_EVENT,
            data,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            new SendOptions { Reliability = false }
        );
        Debug.Log($"[AR Sim] Sent coords: {p} RotY: {y}");
    }
}
