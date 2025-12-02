using UnityEngine;

public class ARFireApplier : MonoBehaviour
{
    [Header("Lista svih fire objekata u istom redoslijedu kao u SimulationMenu")]
    public GameObject[] fireObjects; 

    private void Start()
    {
        int len = fireObjects == null ? 0 : fireObjects.Length;

        if (fireObjects != null)
        {
            for (int i = 0; i < fireObjects.Length; i++)
            {
                string objName = fireObjects[i] ? fireObjects[i].name : "NULL";
            }
        }

        if (GameState.SelectedFireIndices == null)
        {
            Debug.Log("[ARFireApplier] GameState.SelectedFireIndices = NULL");
        }
        else
        {
            Debug.Log("[ARFireApplier] GameState.SelectedFireIndices = " +
                      (GameState.SelectedFireIndices.Length == 0
                          ? "prazno"
                          : string.Join(",", GameState.SelectedFireIndices)));
        }

        // 1) Ugasi sve vatre
        for (int i = 0; i < len; i++)
        {
            if (fireObjects[i] != null)
                fireObjects[i].SetActive(false);
        }

        // 2) Ako PC dio nije postavio ništa, nema što paliti
        if (GameState.SelectedFireIndices == null || GameState.SelectedFireIndices.Length == 0)
        {
            Debug.Log("[ARFireApplier] Nema odabranih požara.");
            return;
        }

        // 3) Upali samo označene požare
        foreach (int id in GameState.SelectedFireIndices)
        {
            int index = id - 1;

            if (index >= 0 && index < len)
            {
                if (fireObjects[index] != null)
                {
                    fireObjects[index].SetActive(true);
                    Debug.Log($"[ARFireApplier] Palim vatru ID={id}, index={index}, obj={fireObjects[index].name}");
                }
                else
                {
                    Debug.LogWarning($"[ARFireApplier] fireObjects[{index}] je NULL za ID={id}");
                }
            }
            else
            {
                Debug.LogWarning($"[ARFireApplier] Fire ID izvan rangea → {id}, index={index}, len={len}");
            }
        }
    }
}
