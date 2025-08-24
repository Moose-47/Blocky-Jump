using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;
    [Space]
    public Slider masterSlider;
    public TMP_Text masterTxt;
    [Space]
    public Slider musicSlider;
    public TMP_Text musicTxt;
    [Space]
    public Slider sfxSlider;
    public TMP_Text sfxTxt;
    [Space]
    public Toggle muteToggle;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    [Header("Gameplay")]
    public Slider tiltSensitivitySlider;
    public TMP_Text tiltSensitivityTxt;

    private SoundManager sm;
    private void Start()
    {
        LoadSettings();
        sm = FindAnyObjectByType<SoundManager>();

        // Setup sliders
        SetupSlider(masterSlider, masterTxt, "Master");
        SetupSlider(musicSlider, musicTxt, "Music");
        SetupSlider(sfxSlider, sfxTxt, "SFX");

        // Setup toggles
        muteToggle.onValueChanged.AddListener(SetMute);
        musicToggle.onValueChanged.AddListener(SetMusicToggle);
        sfxToggle.onValueChanged.AddListener(SetSfxToggle);

        tiltSensitivitySlider.onValueChanged.AddListener(SetTiltSensitivity);

        EventTrigger trigger = sfxSlider.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = sfxSlider.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => OnSfxSliderReleased());
        trigger.triggers.Add(entry);
    }

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();

        muteToggle.onValueChanged.RemoveAllListeners();
        musicToggle.onValueChanged.RemoveAllListeners();
        sfxToggle.onValueChanged.RemoveAllListeners();

        tiltSensitivitySlider.onValueChanged.RemoveAllListeners();
    }

    #region Audio
    private void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        float dB = (value == 0f) ? -80f : 20f * Mathf.Log10(value);
        sliderText.text = (value == 0f) ? "0%" : $"{(int)(value * 100)}%";

        mixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, value);
    }

    private void SetupSlider(Slider slider, TMP_Text sliderText, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
        OnSliderValueChanged(slider.value, slider, sliderText, parameterName);
    }

    private void OnSfxSliderReleased()
    {
        sm.CreateSound(sm.playerJump);
    }

    private void SetMute(bool muted)
    {
        PlayerPrefs.SetInt("Muted", muted ? 1 : 0);
        AudioListener.pause = muted;
    }

    private void SetMusicToggle(bool enabled)
    {
        PlayerPrefs.SetInt("MusicToggle", enabled ? 1 : 0);
        mixer.SetFloat("Music", enabled ? -80f : 0f); //instantly mute/unmute music
    }

    private void SetSfxToggle(bool enabled)
    {
        PlayerPrefs.SetInt("SfxToggle", enabled ? 1 : 0);
        mixer.SetFloat("SFX", enabled ? -80f : 0f); //instantly mute/unmute SFX
    }
    #endregion
    #region Gameplay
    private void SetTiltSensitivity(float value)
    {
        PlayerPrefs.SetFloat("TiltSensitivity", value);
        tiltSensitivityTxt.text = value.ToString("F1");

        var player = FindAnyObjectByType<PlayerController>();
        if (player != null)
            player.tiltSensitivity = value;
    }
    #endregion
    
    private void LoadSettings()
    {
        float defaultVolume = 0.5f;

        // Load volumes
        float masterVol = PlayerPrefs.GetFloat("Master", defaultVolume);
        float musicVol = PlayerPrefs.GetFloat("Music", defaultVolume);
        float sfxVol = PlayerPrefs.GetFloat("SFX", defaultVolume);

        masterSlider.value = masterVol;
        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;

        mixer.SetFloat("Master", masterVol == 0f ? -80f : 20f * Mathf.Log10(masterVol));
        mixer.SetFloat("Music", musicVol == 0f ? -80f : 20f * Mathf.Log10(musicVol));
        mixer.SetFloat("SFX", sfxVol == 0f ? -80f : 20f * Mathf.Log10(sfxVol));

        // Load toggles
        muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt("MusicToggle", 0) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SfxToggle", 0) == 1;

        // Load tilt sensitivity
        float sensitivity = PlayerPrefs.GetFloat("TiltSensitivity", 2f);
        tiltSensitivitySlider.value = sensitivity;
        tiltSensitivityTxt.text = sensitivity.ToString("F1");

        var player = FindAnyObjectByType<PlayerController>();
        if (player != null)
            player.tiltSensitivity = sensitivity;
    }
}
