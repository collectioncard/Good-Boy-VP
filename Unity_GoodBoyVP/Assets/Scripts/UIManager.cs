using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public Image displayImage;
    public Sprite defaultSprite;
    public Sprite testSprite;

    public TextMeshProUGUI messageText;  
    public string defaultMessage = "What da dog doin'?";
    public string testMessage = "Doggo confused";

    void Start() {
        if (displayImage != null && defaultSprite != null) {
            displayImage.sprite = defaultSprite;
        }

        if (messageText != null) {
            messageText.text = defaultMessage;
        }
    }

    public void ChangeToNewImage(Sprite newSprite) {
        if (displayImage != null && newSprite != null) {
            displayImage.sprite = newSprite;
        }
        else {
            displayImage.sprite = testSprite;
        }
    }

    public void ChangeToNewMessage(string newMessage) {
        if (messageText != null) {
            if (!string.IsNullOrEmpty(newMessage)) {
                messageText.text = newMessage;
            }
            else {
                messageText.text = testMessage;
            }
        }
    }
}