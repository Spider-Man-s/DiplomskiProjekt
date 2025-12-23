using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
public class NetworkPlayerOverviewDriver : MonoBehaviour, IOnEventCallback
{
    /*
    [SerializeField] private OverviewManager overview;

    [Header("World bounds of the AR play area")]
    public Vector2 worldMin = new Vector2(-5f, -5f);   // X , Z
    public Vector2 worldMax = new Vector2(5f, 5f);

    [Header("Smoothing")]
    public float smooth = 8f;

    private Vector2 currentPos;
    private float currentRot;

    void Update()
    {
        if (overview == null) return;

        Vector3 worldPos = PlayerNetworkState.Position;
        float rotY = PlayerNetworkState.RotationY;

        // --- 1) Convert WORLD → NORMALIZED 0..1 ---
        float nx = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
        float nz = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.z);

        // Clamp to prevent icon leaving map
        nx = Mathf.Clamp01(nx);
        nz = Mathf.Clamp01(nz);

        // --- 2) Smooth icon movement ---
        Vector2 target = new Vector2(nx, nz);
        currentPos = Vector2.Lerp(currentPos, target, Time.deltaTime * smooth);

        // --- 3) Apply to UI ---
        overview.SetPlayerIconNormalizedPosition(currentPos);

        // Convert world Y rotation → UI rotation
        float targetRot = -rotY; // UI rotates opposite direction
        currentRot = Mathf.LerpAngle(currentRot, targetRot, Time.deltaTime * smooth);
        overview.SetPlayerIconRotation(currentRot);
    }
    */

    const byte POSITION_EVENT = 10;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != POSITION_EVENT) return;

        object[] data = (object[])photonEvent.CustomData;

        Vector3 pos = new Vector3((float)data[0], (float)data[1], (float)data[2]);
        float rotY = (float)data[3];

        PlayerNetworkState.Position = pos;
        PlayerNetworkState.RotationY = rotY;
        PlayerNetworkState.HasData = true;
        Debug.Log($"[PC Scene] Received coords: {pos} RotY: {rotY}");
    }
}
