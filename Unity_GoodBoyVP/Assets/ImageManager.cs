using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public Image displayImage;
    public Sprite defaultSprite;
    public Sprite newSprite;

    void Start()
    {
        if (displayImage != null && defaultSprite != null)
        {
            displayImage.sprite = defaultSprite;
        }
    }

    public void ChangeToNewImage()
    {
        if (displayImage != null && newSprite != null)
        {
            displayImage.sprite = newSprite;
        }
    }
}
