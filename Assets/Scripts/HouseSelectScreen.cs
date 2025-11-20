using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HouseSelectScreen : MonoBehaviour
{
    [Header("UI References")]
    public Button backButton;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Button selectButton;
    public Button previewButton;

    [Header("Carousel")]
    public Transform carouselContainer;
    public GameObject cardPrefab;
    public float cardSpacing = 300f;
    public float animationDuration = 0.3f;

    private List<HouseCard> cards = new List<HouseCard>();
    private int currentIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        leftArrowButton.onClick.AddListener(OnLeftArrowClicked);
        rightArrowButton.onClick.AddListener(OnRightArrowClicked);
        selectButton.onClick.AddListener(OnSelectClicked);
        previewButton.onClick.AddListener(OnPreviewClicked);

        InitializeCarousel();
    }

    void InitializeCarousel()
    {
        HouseData[] houses = UIManager.Instance.houses;

        for (int i = 0; i < houses.Length; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, carouselContainer);
            HouseCard card = cardObj.GetComponent<HouseCard>();
            card.Initialize(houses[i]);
            cards.Add(card);
        }

        UpdateCarouselPositions(false);
    }

    void OnLeftArrowClicked()
    {
        if (isAnimating || cards.Count == 0) return;

        currentIndex = (currentIndex - 1 + cards.Count) % cards.Count;
        UpdateCarouselPositions(true);
    }

    void OnRightArrowClicked()
    {
        if (isAnimating || cards.Count == 0) return;

        currentIndex = (currentIndex + 1) % cards.Count;
        UpdateCarouselPositions(true);
    }

    void UpdateCarouselPositions(bool animate)
    {
        if (animate)
        {
            StartCoroutine(AnimateCarousel());
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                UpdateCardPosition(i, false);
            }
        }

        UIManager.Instance.SetSelectedHouseIndex(currentIndex);
    }

    IEnumerator AnimateCarousel()
    {
        isAnimating = true;
        float elapsed = 0f;

        Vector3[] startPositions = new Vector3[cards.Count];
        Vector3[] startScales = new Vector3[cards.Count];
        float[] startAlphas = new float[cards.Count];

        for (int i = 0; i < cards.Count; i++)
        {
            startPositions[i] = cards[i].rectTransform.anchoredPosition;
            startScales[i] = cards[i].rectTransform.localScale;
            startAlphas[i] = cards[i].canvasGroup.alpha;
        }

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            t = Mathf.SmoothStep(0f, 1f, t); // Smooth easing

            for (int i = 0; i < cards.Count; i++)
            {
                int offset = i - currentIndex;
                Vector3 targetPos = new Vector3(offset * cardSpacing, 0, 0);
                float targetScale = (i == currentIndex) ? 1.2f : 0.8f;
                float targetAlpha = (i == currentIndex) ? 1f : 0.5f;

                cards[i].rectTransform.anchoredPosition = Vector3.Lerp(startPositions[i], targetPos, t);
                cards[i].rectTransform.localScale = Vector3.Lerp(startScales[i], Vector3.one * targetScale, t);
                cards[i].canvasGroup.alpha = Mathf.Lerp(startAlphas[i], targetAlpha, t);
            }

            yield return null;
        }

        // Ensure final positions are exact
        for (int i = 0; i < cards.Count; i++)
        {
            UpdateCardPosition(i, false);
        }

        isAnimating = false;
    }

    void UpdateCardPosition(int index, bool animate)
    {
        int offset = index - currentIndex;
        Vector3 targetPos = new Vector3(offset * cardSpacing, 0, 0);
        float targetScale = (index == currentIndex) ? 1.2f : 0.8f;
        float targetAlpha = (index == currentIndex) ? 1f : 0.5f;

        cards[index].rectTransform.anchoredPosition = targetPos;
        cards[index].rectTransform.localScale = Vector3.one * targetScale;
        cards[index].canvasGroup.alpha = targetAlpha;
    }

    void OnBackClicked()
    {
        UIManager.Instance.ShowStartScreen();
    }

    void OnSelectClicked()
    {
        UIManager.Instance.ShowSimulationPrepScreen();
    }

    void OnPreviewClicked()
    {
        UIManager.Instance.ShowPreviewScreen();
    }
}