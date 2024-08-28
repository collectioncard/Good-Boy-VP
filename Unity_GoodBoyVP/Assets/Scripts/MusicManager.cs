using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip[] musicClips;
    private int currentTrackIndex = 0;
    public bool isPaused = false;

    public Button playButton;
    public Button nextButton;
    private TextMeshProUGUI playButtonText;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Load all music clips from the Resources/Music folder
        musicClips = Resources.LoadAll<AudioClip>("Music");

        if (musicClips.Length > 0) {
            audioSource.clip = musicClips[currentTrackIndex];
        }

        playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
        nextButton = GameObject.Find("NextButton")?.GetComponent<Button>();

        if (playButton != null)
        {
            playButtonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        // Check if the current track has finished playing
        if (!audioSource.isPlaying && !isPaused)
        {
            NextTrack();
        }
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            isPaused = true;
            audioSource.Pause();
            UpdatePlayButtonText("Play");
        }
        else
        {
            isPaused = false;
            audioSource.Play();
            UpdatePlayButtonText("Pause");
        }
    }

    public void NextTrack()
    {
        if (musicClips.Length > 0) {
            currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length;
            audioSource.clip = musicClips[currentTrackIndex];
            audioSource.Play();
            UpdatePlayButtonText("Pause");
        }
    }

    private void UpdatePlayButtonText(string text)
    {
        if (playButtonText != null)
        {
            playButtonText.text = text;
        }
    }
}
