using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerMainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsMenu;
    [SerializeField]
    private SettingsLevels _settings;
    [SerializeField]
    private Slider _volumeSlider;
    [SerializeField]
    private List<AudioSource> _sounds;

    private static UIManagerMainMenu _instance;
    public static UIManagerMainMenu Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        foreach (AudioSource _sound in _sounds)
        {
            _sound.volume = _settings.SoundLevels;
        }
        _volumeSlider.value = _settings.SoundLevels;
    }

    public void Play()
    {
        SceneManager.LoadScene("ChessGameLoop");
    }

    public void Replay()
    {
        SceneManager.LoadScene("ChessReplay");
    }

    public void Settings()
    {
        _settingsMenu.SetActive(true);
    }

    public void VolumeChanged()
    {
        foreach (AudioSource _sound in _sounds)
        {
            _sound.volume = _volumeSlider.value;
        }
        _settings.SoundLevels = _volumeSlider.value;
    }

    public void ReturnFromSettings()
    {
        _settingsMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
