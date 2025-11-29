using UnityEngine;
using UnityEngine.SceneManagement;

public class HousePreviewUI : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] private Transform spawnPoint;   // mjesto gdje će se kuća pojaviti
    [SerializeField] private GameObject[] housePrefabs;

    private Transform currentModel;

    private void Start()
    {
        if (housePrefabs == null || housePrefabs.Length == 0)
        {
            Debug.LogError("Nema prefaba u housePrefabs!");
            return;
        }

        // Učitaj odabranu kuću iz GameState
        int index = Mathf.Clamp(GameState.SelectedHouseIndex, 0, housePrefabs.Length - 1);

        // Spawna kuću na spawn pointu
        GameObject instance = Instantiate(
            housePrefabs[index],
            spawnPoint.position,
            spawnPoint.rotation
        );

        currentModel = instance.transform;

        // Pošalji referencu orbit kameri
        OrbitCamera orbit = Camera.main.GetComponent<OrbitCamera>();
        if (orbit != null)
        {
            orbit.target = currentModel;     // kamera prati ovu kuću
        }
        else
        {
            Debug.LogWarning("OrbitCamera nije pronađen na Main Camera!");
        }
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
