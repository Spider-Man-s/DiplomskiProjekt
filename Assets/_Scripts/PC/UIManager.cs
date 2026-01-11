using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Screen References")]
    public GameObject startScreen;
    public GameObject houseSelectScreen;
    public GameObject previewScreen;
    public GameObject simulationPrepScreen;
    public GameObject simulationScreen;
    public GameObject statisticsScreen;

    [Header("House Data")]
    public HouseData[] houses;

    private int selectedHouseIndex = 0;
    private bool simulationPrepared = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ShowStartScreen();
    }

    public void ShowStartScreen()
    {
        HideAllScreens();
        startScreen.SetActive(true);
    }

    public void ShowHouseSelectScreen()
    {
        HideAllScreens();
        houseSelectScreen.SetActive(true);
    }

    public void ShowPreviewScreen()
    {
        HideAllScreens();
        previewScreen.SetActive(true);
    }

    public void ShowSimulationPrepScreen()
    {
        HideAllScreens();
        simulationPrepared = false;
        simulationPrepScreen.SetActive(true);
    }

    public void ShowSimulationScreen()
    {
        HideAllScreens();
        simulationScreen.SetActive(true);
    }

    public void ShowStatisticsScreen()
    {
        HideAllScreens();
        statisticsScreen.SetActive(true);
    }

    private void HideAllScreens()
    {
        startScreen.SetActive(false);
        houseSelectScreen.SetActive(false);
        previewScreen.SetActive(false);
        simulationPrepScreen.SetActive(false);
        simulationScreen.SetActive(false);
        statisticsScreen.SetActive(false);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public HouseData GetSelectedHouse()
    {
        return houses[selectedHouseIndex];
    }

    public void SetSelectedHouseIndex(int index)
    {
        selectedHouseIndex = index;
    }

    public void SetSimulationPrepared(bool prepared)
    {
        simulationPrepared = prepared;
    }

    public bool IsSimulationPrepared()
    {
        return simulationPrepared;
    }
}