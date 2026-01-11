using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

[System.Serializable]
public class HouseTrackingData
{
    public string houseName;

    [Header("UI References")]
    public RectTransform houseRoot;
    public RectTransform playerIcon;
    public RectTransform spawnAnchor;

    [Header("World Bounds for AR space")]
    public float worldMinX;
    public float worldMaxX;
    public float worldMinZ;
    public float worldMaxZ;

    [Header("Orientation")]
    [Tooltip("Degrees to rotate world space so house forward becomes +Z")]
    public float houseYaw;
    [Header("UI Icon Alignment")]
    [Tooltip("Rotation offset so icon faces forward on this house map")]
    public float iconFacingOffset;
    [Header("Axis Mapping")]
    public bool swapXZ;
    [Header("Axis Inversion")]
    public bool invertWorldX;
    public bool invertWorldZ;
    [Header("UI Axis Inversion")]
    public bool invertUIX;
    public bool invertUIZ;
}
public class NetworkPlayerOverviewDriver : MonoBehaviour, IOnEventCallback
{
    [Header("Houses UI + Bounds Config")]
    public HouseTrackingData[] houses;

    [Header("Smoothing")]
    public float moveSmooth = 10f;
    public float rotateSmooth = 10f;



    Vector2 currentUIPos;
    float currentRot;

    int activeHouse = GameState.SelectedHouseIndex;
    bool hasData = false;


    Vector3 lastWorldPos;
    float lastRotY;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != SimEvents.POSITION_EVENT) return;

        object[] data = (object[])photonEvent.CustomData;

        lastWorldPos = new Vector3(
            (float)data[0],
            (float)data[1],
            (float)data[2]
        );

        lastRotY = (float)data[3];
        hasData = true;


        // Debug.Log($"[NetworkPlayerOverviewDriver] Position event received | Pos=({lastWorldPos.x}, {lastWorldPos.y}, {lastWorldPos.z}) RotY={lastRotY} ActiveHouse={activeHouse}");

    }

    void Update()
    {

        if (!hasData) return;
        if (activeHouse < 0 || activeHouse >= houses.Length) return;

        HouseTrackingData h = houses[activeHouse];
        if (!h.houseRoot || !h.playerIcon) return;

        MovePlayerIcon(h);
        RotatePlayerIcon(h);
    }

    void MovePlayerIcon(HouseTrackingData h)
    {
        Rect r = h.houseRoot.rect;

        float wx = lastWorldPos.x;
        float wz = lastWorldPos.z;

        if (h.invertWorldZ)
            wz *= -1f;
        if (h.invertWorldX)
            wx *= -1f;
        float sourceX = h.swapXZ ? wz : wx;
        float sourceZ = h.swapXZ ? wx : wz;

        float nx = Mathf.InverseLerp(h.worldMinX, h.worldMaxX, sourceX);
        float nz = Mathf.InverseLerp(h.worldMinZ, h.worldMaxZ, sourceZ);

        nx = Mathf.Clamp01(nx);
        nz = Mathf.Clamp01(nz);
        if (h.invertUIX)
            nx = 1f - nx;
        if (h.invertUIZ)
            nz = 1f - nz;

        Vector2 targetPos = new Vector2(
            Mathf.Lerp(-r.width * 0.5f, r.width * 0.5f, nx),
            Mathf.Lerp(-r.height * 0.5f, r.height * 0.5f, nz)
        );


        currentUIPos = Vector2.Lerp(
            currentUIPos,
            targetPos,
            Time.deltaTime * moveSmooth
        );


        h.playerIcon.anchoredPosition = currentUIPos;
    }


    void RotatePlayerIcon(HouseTrackingData h)
    {
        float targetRot =
            -(lastRotY + h.houseYaw) + h.iconFacingOffset;

        currentRot = Mathf.LerpAngle(
            currentRot,
            targetRot,
            Time.deltaTime * rotateSmooth
        );

        h.playerIcon.localEulerAngles = new Vector3(0f, 0f, currentRot);
    }
}

