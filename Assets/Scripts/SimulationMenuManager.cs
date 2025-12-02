using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class HouseEntry
    {
        public GameObject houseRoot;        
        public GameObject functionsRoot;   
    }

    [Header("Sve kuće koje se mogu simulirati")]
    [SerializeField] private HouseEntry[] houses;

    private ActivateFire activeActivateFire;

    [SerializeField] private GameObject infoPopup;


    private void Start()
    {
        int housesCount = (houses == null) ? 0 : houses.Length;
        Debug.Log($"[SimulationMenuManager] Start, GameState.SelectedHouseIndex={GameState.SelectedHouseIndex}, houses.Length={housesCount}");

        if (houses == null || houses.Length == 0)
        {
            Debug.LogError("[SimulationMenuManager] Nema kuća u SimulationMenuManager!");
            return;
        }

        int index = Mathf.Clamp(GameState.SelectedHouseIndex, 0, houses.Length - 1);

        for (int i = 0; i < houses.Length; i++)
        {
            string hrName = houses[i].houseRoot ? houses[i].houseRoot.name : "NULL";
            string frName = houses[i].functionsRoot ? houses[i].functionsRoot.name : "NULL";
            bool active = (i == index);

            if (houses[i].houseRoot != null)
                houses[i].houseRoot.SetActive(active);

            if (houses[i].functionsRoot != null)
                houses[i].functionsRoot.SetActive(active);

            if (active && houses[i].functionsRoot != null)
            {
                activeActivateFire = houses[i].functionsRoot.GetComponent<ActivateFire>();
            }
        }

        Debug.Log($"[SimulationMenuManager] activeActivateFire final = {activeActivateFire}");

        if (activeActivateFire == null)
        {
            Debug.LogWarning("[SimulationMenuManager] Nije pronađen ActivateFire za odabranu kuću!");
        }
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnLockSelection()
    {
        Debug.Log($"[SimulationMenuManager] OnLockSelection called, activeActivateFire={activeActivateFire}, GO={(activeActivateFire ? activeActivateFire.gameObject.name : "null")}");

        if (activeActivateFire == null)
        {
            Debug.LogWarning("[SimulationMenuManager] Nema ActivateFire instance za zaključavanje!");
            return;
        }

        // 1) uzmi koje su točke crvene iz aktivne kuće
        int[] selected = activeActivateFire.GetSelectedFireIndices();

        // 2) spremi u GameState da drugi dijelovi appa znaju
        GameState.SelectedFireIndices = selected;

        Debug.Log("[SimulationMenuManager] LOCK SELECTION → Fire IDs: " +
                  (selected.Length == 0 ? "nema" : string.Join(",", selected)));
        SceneManager.LoadScene("SimulationOverview");
    }

    public void OnInfoButton()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(true);
        }
    }

    public void OnCloseInfoButton()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(false);
        }
    }

}
