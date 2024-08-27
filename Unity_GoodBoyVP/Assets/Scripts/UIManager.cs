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

    public Slider hungerSlider;
    public Slider healthSlider;
    public Slider happySlider;
    public Slider tiredSlider;
    public Slider sickSlider;

    void Start() {
        if (displayImage != null && displayImage.sprite == null && defaultSprite != null) {
            displayImage.sprite = defaultSprite;
        }

        if (messageText != null) {
            messageText.text = defaultMessage;
        }

        hungerSlider = GameObject.Find("HungerSlider")?.GetComponent<Slider>();
        healthSlider = GameObject.Find("HealthSlider")?.GetComponent<Slider>();
        happySlider = GameObject.Find("HappySlider")?.GetComponent<Slider>();
        tiredSlider = GameObject.Find("TiredSlider")?.GetComponent<Slider>();
        sickSlider = GameObject.Find("SickSlider")?.GetComponent<Slider>();
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

    public void UpdateStatBars(DogState stats) {
       if (hungerSlider != null) {
            hungerSlider.value = Mathf.Clamp(stats.HungerLevel, hungerSlider.minValue, hungerSlider.maxValue);
        }

        if (healthSlider != null) {
            healthSlider.value = Mathf.Clamp(stats.Health, healthSlider.minValue, healthSlider.maxValue);
        }

        if (happySlider != null) {
            happySlider.value = Mathf.Clamp(stats.Happiness, happySlider.minValue, happySlider.maxValue);
        }

        if (tiredSlider != null) {
            tiredSlider.value = Mathf.Clamp(stats.TiredLevel, tiredSlider.minValue, tiredSlider.maxValue);
        }

        if (sickSlider != null) {
            sickSlider.value = Mathf.Clamp(stats.SickChance, sickSlider.minValue, sickSlider.maxValue);
        }
    }

}