using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections;

public class ActivateFire : MonoBehaviour
{
    [SerializeField] private List<GameObject> flames = new List<GameObject>();
    [SerializeField] private Color unselectedColor = Color.green;
    [SerializeField] private Color selectedColor = Color.red;

    private bool[] isSelected;
    private void Awake()
    {
        isSelected = new bool[flames.Count];

        for (int i = 0; i < flames.Count; i++)
        {
            if (flames[i] == null) continue;

            flames[i].SetActive(true);
            var img = flames[i].GetComponentInChildren<Image>();
            if (img != null) img.color = unselectedColor;
        }
    }
    public void ToggleFlame(int flameID)
    {
        int index = flameID - 1;
        Debug.Log($"[ActivateFire:{name}] ToggleFlame called with flameID={flameID}, index={index}");

        if (index < 0 || index >= flames.Count)
        {
            Debug.LogWarning($"[ActivateFire:{name}] ToggleFlame index izvan rangea: {index}");
            return;
        }

        isSelected[index] = !isSelected[index];

        if (flames[index] != null)
        {
            var img = flames[index].GetComponentInChildren<Image>();
            if (img != null)
                img.color = isSelected[index] ? selectedColor : unselectedColor;
        }

    }
    public void DeActivateFlame(int flameID)
    {
        int index = flameID - 1;

        if (index < 0 || index >= flames.Count)
        {
            Debug.LogWarning($"[ActivateFire:{name}] DeActivateFlame index out of range: {index}");
            return;
        }

        if (flames[index] != null)
        {
            flames[index].SetActive(false);
            Debug.Log($"[ActivateFire:{name}] Fire {flameID} deactivated.");
        }
    }
    public int[] GetSelectedFireIndices()
    {

        string bits = "";
        for (int i = 0; i < isSelected.Length; i++)
            bits += isSelected[i] ? "1" : "0";


        List<int> selected = new List<int>();

        for (int i = 0; i < isSelected.Length; i++)
            if (isSelected[i])
                selected.Add(i + 1);

        return selected.ToArray();
    }

    public void SendSelectedFiresToAR()
    {
        int[] selected = GetSelectedFireIndices();

        PhotonNetwork.RaiseEvent(
            SimEvents.FIRE_SELECTION,
            selected,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable
        );

        Debug.Log($"[PC] Sent fire selection: {string.Join(",", selected)}");
    }
}