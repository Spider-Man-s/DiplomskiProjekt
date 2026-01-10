using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class ARHouseSpawner : MonoBehaviour, IOnEventCallback
{
    const byte HANDSHAKE_DONE_EVENT_H1 = 31;
    const byte HANDSHAKE_DONE_EVENT_H2 = 32;

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

        Debug.Log($"[ARHouseSpawner] Spawned house {houseIndex}");
    }
}
