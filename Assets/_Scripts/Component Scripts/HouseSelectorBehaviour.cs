using UnityEngine;

public class HouseSelectorBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private ThumbnailBehaviour[] thumbs;
    private ThumbnailBehaviour current_selection;

    private int spacing_x_axis = 715;
    void Start()
    {
        thumbs = GetComponentsInChildren<ThumbnailBehaviour>();
        thumbs[0].SelectThumbnail();
        current_selection = thumbs[0]; 
    }
}
