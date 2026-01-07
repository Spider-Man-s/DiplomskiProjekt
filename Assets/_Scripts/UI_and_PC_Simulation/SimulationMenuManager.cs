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

    [Header("UI")]
    [SerializeField] private GameObject infoPopup;
    [SerializeField] private GameObject warningPopup;

    [Header("Handshake")]
    [SerializeField] private HandshakeController handshake; // NOVO

    private void Start()
    {
        if (houses == null || houses.Length == 0)
        {
            Debug.LogError("[SimulationMenuManager] Nema kuća!");
            return;
        }

        int index = Mathf.Clamp(GameState.SelectedHouseIndex, 0, houses.Length - 1);

        for (int i = 0; i < houses.Length; i++)
        {
            bool active = (i == index);

            if (houses[i].houseRoot != null)
                houses[i].houseRoot.SetActive(active);

            if (houses[i].functionsRoot != null)
                houses[i].functionsRoot.SetActive(active);

            if (active && houses[i].functionsRoot != null)
                activeActivateFire = houses[i].functionsRoot.GetComponent<ActivateFire>();
        }

        if (infoPopup != null) infoPopup.SetActive(false);
        if (warningPopup != null) warningPopup.SetActive(false);
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // gumb "Start sim"
    public void OnLockSelection()
    {
        if (activeActivateFire == null)
            return;

        int[] selected = activeActivateFire.GetSelectedFireIndices();

        if (selected == null || selected.Length == 0)
        {
            if (warningPopup != null)
                warningPopup.SetActive(true);
            return;
        }

        GameState.SelectedFireIndices = selected;

        // Start handshake gate
        if (handshake != null)
            handshake.BeginHandshake();
        else
            Debug.LogError("[SimulationMenuManager] HandshakeController nije povezan u Inspectoru!");
    }

    // POPUPS
    public void OnInfoButton()
    {
        if (infoPopup != null)
            infoPopup.SetActive(true);
    }

    public void OnCloseInfoButton()
    {
        if (infoPopup != null)
            infoPopup.SetActive(false);
    }

    public void OnCloseWarningPopup()
    {
        if (warningPopup != null)
            warningPopup.SetActive(false);
    }
}
