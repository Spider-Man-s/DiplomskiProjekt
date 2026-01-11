using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class ARHouseSpawner : MonoBehaviour, IOnEventCallback
{
    const byte HANDSHAKE_DONE_EVENT_H1 = 31;
    const byte HANDSHAKE_DONE_EVENT_H2 = 32;
    private const byte EVENT_FIRE_SELECTION = 22;

    [Header("House Prefabs")]
    [SerializeField] GameObject[] housePrefabs;

    [Header("House Spawn Anchors")]
    [SerializeField] Transform[] houseSpawnAnchors;

    GameObject spawnedHouse;
    bool hasSpawned = false;

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EVENT_FIRE_SELECTION)
        {
            ARSimulationState.SelectedFireIds =
                (int[])photonEvent.CustomData;

            Debug.Log("[ARHouseSpawner] Cached fire IDs: " +
                string.Join(",", ARSimulationState.SelectedFireIds));

            return;
        }

        if (photonEvent.Code != HANDSHAKE_DONE_EVENT_H1 &&
            photonEvent.Code != HANDSHAKE_DONE_EVENT_H2)
            return;

        if (hasSpawned)
            return;

        int houseIndex = photonEvent.Code == HANDSHAKE_DONE_EVENT_H1 ? 0 : 1;

        Debug.Log("[ARHouseSpawner] Handshake done event received");
        SpawnSelectedHouse(houseIndex);
        hasSpawned = true;
    }

    public void SpawnSelectedHouse(int houseIndex)
    {
        Debug.Log($"[ARHouseSpawner] Selected house index {houseIndex}");

        if (houseIndex < 0 || houseIndex >= housePrefabs.Length)
        {
            Debug.LogError($"[ARHouseSpawner] Invalid house index {houseIndex}");
            return;
        }

        if (spawnedHouse != null)
            Destroy(spawnedHouse);

        Transform anchor = houseSpawnAnchors[houseIndex];

        spawnedHouse = Instantiate(
            housePrefabs[houseIndex],
            anchor.position,
            anchor.rotation
        );

        ApplyFireSelection(spawnedHouse);

        Debug.Log($"[ARHouseSpawner] Spawned house {houseIndex}");
    }

    void ApplyFireSelection(GameObject house)
    {
        if (ARSimulationState.SelectedFireIds == null)
        {
            Debug.LogWarning("[AR] No fire selection cached yet");
            return;
        }

        FireZone[] zones =
            house.GetComponentsInChildren<FireZone>(true);

        foreach (var zone in zones)
            zone.SetActive(false);

        foreach (int fireId in ARSimulationState.SelectedFireIds)
        {
            foreach (var zone in zones)
            {
                if (zone.fireId == fireId)
                {
                    zone.SetActive(true);
                    Debug.Log($"[AR] Activated FireZone {fireId}");
                }
            }
        }
    }


}



public static class ARSimulationState
{
    public static int[] SelectedFireIds;
}