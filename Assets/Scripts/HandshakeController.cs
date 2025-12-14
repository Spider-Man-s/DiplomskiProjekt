using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HandshakeController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private TMP_Text waitingStatusText;

    [Header("Scenes")]
    [SerializeField] private string menuSceneName = "SimulationMenu";
    [SerializeField] private string overviewSceneName = "SimulationOverview";

    // handshake state
    private bool isWaiting = false;
    private bool arConnected = false;
    private bool arAtStart = false;
    private bool arReady = false;

    private void Awake()
    {
        if (waitingPanel != null)
            waitingPanel.SetActive(false);

        UpdateWaitingUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Ako se sceneLoaded “promaši” iz bilo kojeg razloga, Start je fallback (ali i dalje čist).
        TryAutoBegin("Start");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != menuSceneName)
            return;

        TryAutoBegin("sceneLoaded");
    }

    private void TryAutoBegin(string source)
    {
        // Debug koji ti odmah kaže zašto se ne pali
        int firesLen = (GameState.SelectedFireIndices == null) ? -1 : GameState.SelectedFireIndices.Length;
        Debug.Log($"[HandshakeController] TryAutoBegin from {source}: ReplayRequested={GameState.ReplayRequested}, SelectedFireIndicesLen={firesLen}");

        if (!GameState.ReplayRequested)
            return;

        // Ovdje ga odmah spuštamo da ne uđe u loop, ali samo ako imamo fire listu
        if (GameState.SelectedFireIndices != null && GameState.SelectedFireIndices.Length > 0)
        {
            GameState.ReplayRequested = false;
            BeginHandshake();
        }
        else
        {
            // Replay je tražen, ali nema spremljenih požara -> samo ostavi flag (ili ga ugasi, po želji)
            Debug.LogWarning("[HandshakeController] ReplayRequested je true, ali SelectedFireIndices je null/0. Handshake se ne može auto-pokrenuti.");
        }
    }

    // Poziva se kad user klikne "Start sim" nakon validacije požara
    public void BeginHandshake()
    {
        isWaiting = true;

        // reset uvjeta koji trebaju ponovno vrijediti
        arAtStart = false;
        arReady = false;

        if (waitingPanel != null)
        {
            waitingPanel.SetActive(true);
            waitingPanel.transform.SetAsLastSibling(); // iznad svega
        }
        else
        {
            Debug.LogError("[HandshakeController] waitingPanel nije povezan u Inspectoru!");
        }

        UpdateWaitingUI();
        TryProceed();
    }

    public void CancelHandshake()
    {
        isWaiting = false;

        if (waitingPanel != null)
            waitingPanel.SetActive(false);

        arAtStart = false;
        arReady = false;

        UpdateWaitingUI();
    }

#if UNITY_EDITOR
    private void Update()
    {
        // TEST TIPKE
        if (Input.GetKeyDown(KeyCode.Y)) SetARConnected(true);
        if (Input.GetKeyDown(KeyCode.X)) SetARAtStartPosition(true);
        if (Input.GetKeyDown(KeyCode.W)) ConfirmARReady();
        if (Input.GetKeyDown(KeyCode.R)) CancelHandshake();
    }
#endif

    // OVE METODE zove networking sloj na PC-u
    public void SetARConnected(bool connected)
    {
        arConnected = connected;
        UpdateWaitingUI();
        TryProceed();
    }

    public void SetARAtStartPosition(bool atStart)
    {
        arAtStart = atStart;
        UpdateWaitingUI();
        TryProceed();
    }

    public void ConfirmARReady()
    {
        arReady = true;
        UpdateWaitingUI();
        TryProceed();
    }

    private void TryProceed()
    {
        if (!isWaiting) return;

        if (arConnected && arAtStart && arReady)
        {
            SceneManager.LoadScene(overviewSceneName);
        }
    }

    private void UpdateWaitingUI()
    {
        if (waitingStatusText == null) return;

        if (!isWaiting)
        {
            waitingStatusText.text = "";
            return;
        }

        waitingStatusText.text =
            "<b>Čekanje AR korisnika...</b>\n\n" +
            $"• Povezan: {(arConnected ? "<color=green>DA</color>" : "<color=red>NE</color>")}\n" +
            $"• Na start poziciji: {(arAtStart ? "<color=green>DA</color>" : "<color=red>NE</color>")}\n" +
            $"• Potvrda spremnosti: {(arReady ? "<color=green>DA</color>" : "<color=red>NE</color>")}";
    }
}
