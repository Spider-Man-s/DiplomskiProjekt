using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActivateFire : MonoBehaviour
{
    [SerializeField] private List<GameObject> flames = new List<GameObject>();

    [SerializeField] private Color unselectedColor = Color.green;
    [SerializeField] private Color selectedColor   = Color.red;

    private bool[] isSelected;  // pamti koje su točke odabrane

    private void Awake()
    {
        Debug.Log($"[ActivateFire:{name}] Awake, flames.Count = {flames.Count}");

        isSelected = new bool[flames.Count];

        // sve točke na početk uključene i zelene
        for (int i = 0; i < flames.Count; i++)
        {
            if (flames[i] == null)
            {
                
                continue;
            }

            flames[i].SetActive(true);

            var img = flames[i].GetComponentInChildren<Image>();
            if (img != null)
            {
                img.color = unselectedColor;
            }
            
        }
    }

    public void ActivateFlame(int flameID)
    {
        int index = flameID - 1;
        Debug.Log($"[ActivateFire:{name}] ActivateFlame flameID={flameID}, index={index}");

        if (index < 0 || index >= flames.Count)
        {
            Debug.LogWarning($"[ActivateFire:{name}] ActivateFlame index izvan rangea: {index}");
            return;
        }

        if (flames[index] != null)
            flames[index].SetActive(true);
    }

    public void DeActivateFlame(int flameID)
    {
        int index = flameID - 1;
        Debug.Log($"[ActivateFire:{name}] DeActivateFlame flameID={flameID}, index={index}");

        if (index < 0 || index >= flames.Count)
        {
            Debug.LogWarning($"[ActivateFire:{name}] DeActivateFlame index izvan rangea: {index}");
            return;
        }

        if (flames[index] != null)
            flames[index].SetActive(false);
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

            Debug.Log($"[ActivateFire:{name}] isSelected[{index}] = {isSelected[index]} za objekt {flames[index].name}");
        }
        else
        {
            Debug.LogWarning($"[ActivateFire:{name}] flames[{index}] je NULL u ToggleFlame");
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
