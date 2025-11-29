using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class ActivateFire : MonoBehaviour
{
    [SerializeField] private List<GameObject> flames = new List<GameObject>();

    private const byte EVENT_FIRE_ACTIVATED = 1;

    public void ActivateFlame(int flameID)
    {
        if (flames[flameID - 1] != null)
        {
            flames[flameID - 1].SetActive(true);

            string message = $"upaljen je požar broj {flameID}";
            var raiseOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(
                EVENT_FIRE_ACTIVATED,
                message,
                raiseOptions,
                SendOptions.SendReliable
            );
        }
    }

    public void DeActivateFlame(int flameID)
    {
        if (flames[flameID - 1] != null)
        {
            flames[flameID - 1].SetActive(false);
        }
    }
}