using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public class HouseData
    {
        public string houseName;
        public Sprite thumbnail;
        public string previewSceneName;
        public string simulationSceneName;
    }

    [Header("UI References")]
    [SerializeField] private Image houseImage;
    [SerializeField] private TMP_Text houseNameText;

    [Header("Houses")]
    [SerializeField] private HouseData[] houses;

    private int currentIndex = 0;

    private void Start()
    {
        if (houses.Length > 0)
        {
            ShowCurrentHouse();
        }
        else
        {
            Debug.LogWarning("Nema kuća dodanih u MainMenuUI!");
        }
    }

    private void ShowCurrentHouse()
    {
        HouseData h = houses[currentIndex];
        if (houseImage != null) houseImage.sprite = h.thumbnail;
        if (houseNameText != null) houseNameText.text = h.houseName;
    }

    public void OnNextHouse()
    {
        if (houses.Length == 0) return;
        currentIndex = (currentIndex + 1) % houses.Length;
        ShowCurrentHouse();
    }

    public void OnPreviousHouse()
    {
        if (houses.Length == 0) return;
        currentIndex = (currentIndex - 1 + houses.Length) % houses.Length;
        ShowCurrentHouse();
    }

    public void OnPreviewHouse()
    {
        if (houses.Length == 0) return;

        // zapamti koju je kuću odabrao
        GameState.SelectedHouseIndex = currentIndex;

        // svi idu u istu preview scenu
        SceneManager.LoadScene("PreviewScene");
    }

    public void OnSelectHouse()
    {
        if (houses.Length == 0) return;

        GameState.SelectedHouseIndex = currentIndex;
        Debug.Log($"[HouseSELECTION] Selected house {currentIndex}");

        string sceneName = houses[currentIndex].simulationSceneName;
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }

    }
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
