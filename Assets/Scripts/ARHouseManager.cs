using UnityEngine;

public class ARHouseManager : MonoBehaviour
{
    [Header("Sve kuće u AR sceni (redom kao u MainMenu-u)")]
    public GameObject[] houseRoots;   

    private void Start()
    {
        if (houseRoots == null || houseRoots.Length == 0)
        {
            Debug.LogWarning("[ARHouseManager] Nema kuća u listi!");
            return;
        }

        int index = Mathf.Clamp(GameState.SelectedHouseIndex, 0, houseRoots.Length - 1);

        for (int i = 0; i < houseRoots.Length; i++)
        {
            if (houseRoots[i] == null) continue;

            bool active = (i == index);
            houseRoots[i].SetActive(active);

            Debug.Log($"[ARHouseManager] House[{i}] = {houseRoots[i].name}, setActive={active}");
        }
    }
}
