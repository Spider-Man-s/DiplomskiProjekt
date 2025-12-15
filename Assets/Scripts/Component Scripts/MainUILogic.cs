using UnityEngine;

public class MainUILogic : MonoBehaviour
{

    [SerializeField] GameObject MainMenuSelectHouse;
    [SerializeField] GameObject StartScreen;

    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject ExitButton;
    [SerializeField] GameObject SelectButton;
    [SerializeField] GameObject PreviewButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    public void ShowMainMenu() { 
        MainMenuSelectHouse.SetActive(true);
        StartScreen.SetActive(false);
    }

    public void ShowStartMenu()
    {
        MainMenuSelectHouse.SetActive(false);
        StartScreen.SetActive(true);
    }
}
