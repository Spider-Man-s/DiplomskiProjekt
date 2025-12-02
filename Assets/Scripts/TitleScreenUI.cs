using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void OnStartButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("Quit game"); 
    }
}
