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
}
public class NetworkPlayerOverviewDriver : MonoBehaviour, IOnEventCallback
{
    [Header("Houses UI + Bounds Config")]
    public HouseTrackingData[] houses;

    [Header("Smoothing")]
    public float moveSmooth = 10f;
    public float rotateSmooth = 10f;

    const byte POSITION_EVENT = 10;

    Vector2 currentUIPos;
    float currentRot;

    int activeHouse = 0;
    bool hasData = false;
    Vector3 lastWorldPos;
    float lastRotY;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != POSITION_EVENT) return;

        object[] data = (object[])photonEvent.CustomData;

        Vector3 pos = new Vector3(
            (float)data[0],
            (float)data[1],
            (float)data[2]
        );

        float rotY = (float)data[3];

        activeHouse = (int)data[4];

        lastWorldPos = pos;
        lastRotY = rotY;
        hasData = true;

        Debug.Log($"PC received: Pos={pos} Rot={rotY} House={activeHouse}");
    }

    void Update()
    {
        if (!hasData) return;
        if (activeHouse < 0 || activeHouse >= houses.Length) return;

        var h = houses[activeHouse];
        if (h.houseRoot == null || h.playerIcon == null || h.spawnAnchor == null)
            return;

        MovePlayerIcon(h);
        RotatePlayerIcon(h);
    }

    void MovePlayerIcon(HouseTrackingData h)
    {
        RectTransform mapRect = h.houseRoot;
        Rect r = mapRect.rect;

        float zNorm = Mathf.InverseLerp(h.worldMinZ, h.worldMaxZ, lastWorldPos.z);
        float nx = 1f - Mathf.Clamp01(zNorm);


        float xNorm = Mathf.InverseLerp(h.worldMinX, h.worldMaxX, lastWorldPos.x);
        float ny = Mathf.Clamp01(xNorm);

        Vector2 localPos = new Vector2(
            Mathf.Lerp(-r.width / 2f, r.width / 2f, nx),
            Mathf.Lerp(-r.height / 2f, r.height / 2f, ny)
        );

        currentUIPos = Vector2.Lerp(currentUIPos, localPos, Time.deltaTime * moveSmooth);
        h.playerIcon.anchoredPosition = currentUIPos;
    }

    void RotatePlayerIcon(HouseTrackingData h)
    {

        float target = -lastRotY + 90f;

        currentRot = Mathf.LerpAngle(currentRot, target, Time.deltaTime * rotateSmooth);
        h.playerIcon.localEulerAngles = new Vector3(0, 0, currentRot);
    }
}
