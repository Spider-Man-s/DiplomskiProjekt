using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
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

    // ostale tvoje funkcije mogu ostati (ActivateFlame/DeActivateFlame) ako ti trebaju
}