using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
public class ARFireSender
{
    public static void SendFireExtinguished(int fireId)
    {
        PhotonNetwork.RaiseEvent(
            SimEvents.EVENT_FIRE_EXTINGUISHED,
            fireId,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable
        );

        Debug.Log($"[AR] Fire extinguished {fireId}");
    }
}
