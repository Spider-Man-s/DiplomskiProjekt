using UnityEngine;
using UnityEngine.SceneManagement;

public class HousePreviewUI : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] private Transform[] houseSpawnPoints;
    [SerializeField] private GameObject[] housePrefabs;
    [SerializeField] private GameObject infoPopup;
    [SerializeField] private GameObject infoButton;

    private Transform currentModel;

    private void Start()
    {
        if (housePrefabs == null || housePrefabs.Length == 0)
        {
            Debug.LogError("Nema prefaba u housePrefabs!");
            return;
        }

        if (houseSpawnPoints == null || houseSpawnPoints.Length < housePrefabs.Length)
        {
            Debug.LogError("Nema dovoljno spawn pointova za sve kuće!");
            return;
        }

        int index = Mathf.Clamp(
            GameState.SelectedHouseIndex,
            0,
            housePrefabs.Length - 1
        );

        Transform spawnPoint = houseSpawnPoints[index];

        GameObject instance = Instantiate(
            housePrefabs[index],
            spawnPoint.position,
            spawnPoint.rotation
        );

        currentModel = instance.transform;
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnInfoButton()
    {
        if (infoPopup != null)
            infoPopup.SetActive(true);
        if (infoButton != null)
            infoButton.SetActive(false);
    }

    public void OnCloseInfoButton()
    {
        if (infoPopup != null)
            infoPopup.SetActive(false);
        if (infoButton != null)
            infoButton.SetActive(true);
    }
}
