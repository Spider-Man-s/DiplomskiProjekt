using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;



public class OverviewManager : MonoBehaviour
{
    [System.Serializable]
    public class HouseLayoutData
    {
        public GameObject layoutRoot;   // panel kuće
        public TMPro.TextMeshProUGUI debugLabel; 
        public UnityEngine.UI.Image[] fireIcons;       // točke za tu kuću
    }

    [Header("Layouts za sve kuće (redoslijedom iz MainMenu-a)")]
    [SerializeField] private HouseLayoutData[] houseLayouts;

    [Header("Boje")]
    [SerializeField] private Color fireActiveColor = Color.red;
    [SerializeField] private Color fireExtinguishedColor = Color.green;
    [SerializeField] private Color fireHiddenColor = new Color(0, 0, 0, 0);

    [Header("Player icon")]
    [SerializeField] private RectTransform playerIcon;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private GameObject infoPopup;
    [SerializeField] private GameObject exitPopup;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private HouseLayoutData activeLayout;
    private int totalFires;
    private int extinguishedFires;
    private float startTime;

    private void Awake()
    {
       
    }

    private void Start()
    {
        
        if (houseLayouts == null || houseLayouts.Length == 0)
        {
            Debug.LogError("[OverviewManager] houseLayouts je prazan ili null!");
            return;
        }

        SelectCorrectHouseLayout();
        SetupFires();
        StartTimer();

        if (infoPopup != null) infoPopup.SetActive(false);
        if (exitPopup != null) exitPopup.SetActive(false);

    }

    private void SelectCorrectHouseLayout()
    {
        int selected = GameState.SelectedHouseIndex;
        int clamped = Mathf.Clamp(selected, 0, houseLayouts.Length - 1);


        // ugasi sve
        for (int i = 0; i < houseLayouts.Length; i++)
        {
            if (houseLayouts[i].layoutRoot != null)
            {
                houseLayouts[i].layoutRoot.SetActive(false);
                Debug.Log($"[OverviewManager] layoutRoot[{i}] = {houseLayouts[i].layoutRoot.name} -> setActive(false)");
            }
            else
            {
                Debug.LogWarning($"[OverviewManager] layoutRoot[{i}] je NULL");
            }
        }

        activeLayout = houseLayouts[clamped];

        if (activeLayout.layoutRoot != null)
        {
            activeLayout.layoutRoot.SetActive(true);
            Debug.Log($"[OverviewManager] Active layout = {activeLayout.layoutRoot.name} (index {clamped}) -> setActive(true)");
        }
        else
        {
            Debug.LogError("[OverviewManager] Active layoutRoot je NULL!");
        }
    }

    private void SetupFires()
    {
        if (activeLayout == null)
        {
            Debug.LogError("[OverviewManager] SetupFires: activeLayout je NULL!");
            return;
        }

        if (activeLayout.fireIcons == null)
        {
            Debug.LogError("[OverviewManager] SetupFires: activeLayout.fireIcons je NULL!");
            return;
        }

        Debug.Log($"[OverviewManager] SetupFires: aktivna kuća ima {activeLayout.fireIcons.Length} fire ikona.");

        // sakrij sve točke za tu kuću
        for (int i = 0; i < activeLayout.fireIcons.Length; i++)
        {
            if (activeLayout.fireIcons[i] != null)
            {
                activeLayout.fireIcons[i].color = fireHiddenColor;
                Debug.Log($"[OverviewManager] FireIcon[{i}] ({activeLayout.fireIcons[i].name}) -> HIDDEN");
            }
            else
            {
                Debug.LogWarning($"[OverviewManager] FireIcon[{i}] je NULL");
            }
        }

        totalFires = 0;
        extinguishedFires = 0;

        if (GameState.SelectedFireIndices == null || GameState.SelectedFireIndices.Length == 0)
        {
            Debug.LogWarning("[OverviewManager] GameState.SelectedFireIndices je null ili prazan – nema aktivnih požara");
            UpdateCounterText();
            return;
        }

        Debug.Log("[OverviewManager] SelectedFireIndices: " +
                  string.Join(",", GameState.SelectedFireIndices));

        foreach (int id in GameState.SelectedFireIndices)
        {
            int index = id - 1;
            if (index >= 0 && index < activeLayout.fireIcons.Length && activeLayout.fireIcons[index] != null)
            {
                activeLayout.fireIcons[index].color = fireActiveColor;
                Debug.Log($"[OverviewManager] FireIcon ID={id} (index={index}, name={activeLayout.fireIcons[index].name}) -> ACTIVE");
                totalFires++;
            }
            else
            {
                Debug.LogWarning($"[OverviewManager] Fire ID={id} -> index={index} izvan rangea ili ikona NULL.");
            }
        }

        Debug.Log($"[OverviewManager] Ukupno aktivnih požara: {totalFires}");
        UpdateCounterText();
    }

    private void StartTimer()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        float elapsed = Time.time - startTime;
        TimeSpan t = TimeSpan.FromSeconds(elapsed);
        timerText.text = $"{t.Minutes:00}:{t.Seconds:00}";
    }

    private void UpdateCounterText()
    {
        if (counterText == null) return;
        counterText.text = $"Fires extinguished: {extinguishedFires} / {totalFires}";
    }

    public void OnFireExtinguished(int fireId)
    {
        if (activeLayout == null || activeLayout.fireIcons == null)
        {
            Debug.LogWarning("[OverviewManager] OnFireExtinguished: activeLayout/fireIcons null");
            return;
        }

        int index = fireId - 1;
        if (index < 0 || index >= activeLayout.fireIcons.Length) return;

        var icon = activeLayout.fireIcons[index];
        if (icon == null) return;

        if (icon.color == fireActiveColor)
        {
            icon.color = fireExtinguishedColor;
            extinguishedFires++;
            Debug.Log($"[OverviewManager] Požar ugašen: ID={fireId}, index={index}, name={icon.name}");
            UpdateCounterText();
        }

        if (extinguishedFires == totalFires && totalFires > 0)
        {
            FinishSimulationAndShowReport();
        }

    }

    public void ShowInfoPopup()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(true);
        }
    }

    public void HideInfoPopup()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(false);
        }
    }

    public void ShowExitPopup()
    {
        if (exitPopup != null)
        {
            exitPopup.SetActive(true);
        }
    }

    public void HideExitPopup()
    {
        if (exitPopup != null)
        {
            exitPopup.SetActive(false);
        }
    }

    public void ExitSimulation()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // za pomicanje ikonice igrača po layoutu (0–1 range po x,y)
    public void SetPlayerIconNormalizedPosition(Vector2 normalizedPos)
    {
        if (activeLayout == null || playerIcon == null)
        {
            Debug.LogWarning("[OverviewManager] SetPlayerIconNormalizedPosition: missing references");
            return;
        }

        // layoutRect je layoutRoot od aktivne kuće
        RectTransform layoutRect = activeLayout.layoutRoot.GetComponent<RectTransform>();
        if (layoutRect == null)
        {
            Debug.LogWarning("[OverviewManager] LayoutRoot nema RectTransform!");
            return;
        }

        Rect r = layoutRect.rect;

        // normalized (0–1) pretvori u anchoredPosition
        float x = (normalizedPos.x - 0.5f) * r.width;
        float y = (normalizedPos.y - 0.5f) * r.height;

        playerIcon.anchoredPosition = new Vector2(x, y);
    }

    public void SetPlayerIconRotation(float angleDegrees)
    {
        if (playerIcon == null) return;

        // UI rotacija oko Z osi
        playerIcon.localEulerAngles = new Vector3(0f, 0f, angleDegrees);
    }

    public void FinishSimulationAndShowReport()
    {
        // izračun trajanja
        float duration = Time.time - startTime;

        // spremi rezultate u globalnu klasu
        SimulationResults.TotalFires = totalFires;
        SimulationResults.ExtinguishedFires = extinguishedFires;
        SimulationResults.DurationSeconds = duration;

        Debug.Log($"[OverviewManager] FinishSimulation: {extinguishedFires}/{totalFires}, time={duration}s");

        // učitaj report scenu
        SceneManager.LoadScene("SimulationReport");  
    }



}
