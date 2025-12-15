using UnityEngine;
using UnityEngine.UIElements;

public class ThumbnailBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject outline;
    [SerializeField] GameObject inactive_cover;
    void Start()
    {
        outline = GameObject.FindWithTag("ThumbnailOutline");
        inactive_cover = GameObject.FindWithTag("ThumbnailInactive");

        outline.SetActive(false);
        inactive_cover.SetActive(true);
    }

    public void SelectThumbnail() {
        outline.SetActive(true);
        Debug.Log("CALLING SELECT THUMBNAIL");
        inactive_cover.SetActive(false);
    }

    public void DeeelectThumbnail()
    {
        outline.SetActive(false);
        inactive_cover.SetActive(true);
    }

}
