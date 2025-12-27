using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerCoordinates : MonoBehaviourPun
{

    const byte POSITION_EVENT = 10;

    [SerializeField] float sendRate = 10f;
    [SerializeField] TMP_Text debugText;

    float timer;

    void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        if (!photonView.IsMine) return;

        timer += Time.deltaTime;
        if (timer < 1f / sendRate) return;
        timer = 0f;

        Vector3 p = transform.position;
        float y = transform.eulerAngles.y;

        if (debugText)
        {
            string format = "F2";
            debugText.text =
                $"Player Position\n" +
                $"X: {p.x.ToString(format)}\n" +
                $"Z: {p.z.ToString(format)}\n\n" +
                $"Rotation Y: {y.ToString(format)}°";
        }

        object[] data = { p.x, p.y, p.z, y, GameState.SelectedHouseIndex };

        PhotonNetwork.RaiseEvent(
            POSITION_EVENT,
            data,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            new SendOptions { Reliability = false }
        );

        Debug.Log($"[AR Sim] Sent coords: {p} RotY: {y}");
    }
}
