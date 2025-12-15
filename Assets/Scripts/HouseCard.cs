using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class HouseCard : MonoBehaviour
{
    public Image cardImage;
    public TMP_Text houseNameText;

    [HideInInspector]
    public RectTransform rectTransform;
    [HideInInspector]
    public CanvasGroup canvasGroup;

    private HouseData houseData;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(HouseData data)
    {
        houseData = data;
        cardImage.sprite = data.cardImage;
        houseNameText.text = data.houseName;
    }

    public HouseData GetHouseData()
    {
        return houseData;
    }
}