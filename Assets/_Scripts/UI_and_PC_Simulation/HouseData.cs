using UnityEngine;

[CreateAssetMenu(fileName = "HouseData", menuName = "Fireman Training/House Data")]
public class HouseData : ScriptableObject
{
    public string houseName;
    public Sprite cardImage;
    public GameObject housePrefab;
    public Texture2D topViewTexture;
}