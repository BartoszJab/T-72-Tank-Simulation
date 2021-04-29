using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start() {
        float volumeLevel;
        audioMixer.GetFloat("volume", out volumeLevel);
        volumeSlider.value = volumeLevel;
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int graphicsQualityIndex) {
        QualitySettings.SetQualityLevel(graphicsQualityIndex);
    }
}
