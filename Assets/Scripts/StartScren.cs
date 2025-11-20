using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;

    void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    void OnStartClicked()
    {
        UIManager.Instance.ShowHouseSelectScreen();
    }

    void OnExitClicked()
    {
        UIManager.Instance.QuitApplication();
    }
}