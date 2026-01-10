using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
public class ARFireSender
{
    const byte EVENT_FIRE_EXTINGUISHED = 2;

    public static void SendFireExtinguished(int fireId)
    {
        PhotonNetwork.RaiseEvent(
            EVENT_FIRE_EXTINGUISHED,
            fireId,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable
        );

        Debug.Log($"[AR] Fire extinguished {fireId}");
    }
}
