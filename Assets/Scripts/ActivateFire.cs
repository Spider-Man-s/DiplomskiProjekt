using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;  
using ExitGames.Client.Photon;

public class ActivateFire : MonoBehaviour
{
    [SerializeField] private List<GameObject> flames = new List<GameObject>();
    [SerializeField] private Color unselectedColor = Color.green;
    [SerializeField] private Color selectedColor = Color.red;

    private bool[] isSelected;

    private const byte EVENT_FIRE_ACTIVATED = 1;

    private void Awake()
    {
        Debug.Log($"[ActivateFire:{name}] Awake, flames.Count = {flames.Count}");
        isSelected = new bool[flames.Count];

        for (int i = 0; i < flames.Count; i++)
        {
            if (flames[i] == null) continue;

            flames[i].SetActive(true);
            var img = flames[i].GetComponentInChildren<Image>();
            if (img != null) img.color = unselectedColor;
        }
    }

    // OVO TI JE "klik" funkcija - ovdje ćemo i slati poruku kad se upali
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

            Debug.Log($"[ActivateFire:{name}] isSelected[{index}] = {isSelected[index]} za objekt {flames[index].name}");
        }

        // Ako je nakon klika UPALJENO -> šalji na AR
        if (isSelected[index])
        {
            SendFireActivatedEvent(flameID); // šaljemo flameID (1-based)
        }
    }

    private void SendFireActivatedEvent(int flameID)
    {
        // payload kao int (može i object[] ako kasnije dodaš više polja)
        PhotonNetwork.RaiseEvent(
            EVENT_FIRE_ACTIVATED,
            flameID,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable
        );

        Debug.Log($"[ActivateFire:{name}] Sent event: upalila se vatra {flameID}");
    }

    public void ActivateFlame(int flameID)
    {
        int index = flameID - 1;

        if (index < 0 || index >= flames.Count)
        {
            Debug.LogWarning($"[ActivateFire:{name}] ActivateFlame index out of range: {index}");
            return;
        }

        if (flames[index] != null)
        {
            flames[index].SetActive(true);

            // Send event to others
            string message = $"upaljen je požar broj {flameID}";
            PhotonNetwork.RaiseEvent(
                EVENT_FIRE_ACTIVATED,
                message,
                new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable
            );

            Debug.Log($"[ActivateFire:{name}] Fire {flameID} activated + event sent.");
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
}