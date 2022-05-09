using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _settingsMenu;
    [SerializeField]
    private GameObject _gameOverMenu;
    [SerializeField]
    private GameObject _pauseButton;
    [SerializeField]
    private Slider _volumeSlider;
    [SerializeField]
    private List<AudioSource> _sounds;
    [SerializeField]
    private TextMeshProUGUI _winnerText;
    [SerializeField]
    private SettingsLevels _settings;

    private static UIManager _instance;
    public static UIManager Instance { get => _instance; }

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

    public void Pause()
    {
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
    }

    public void Settings()
    {
        _settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        _settingsMenu.SetActive(false);
    }

    public void Save()
    {

    }

    public void ReturnFromSave()
    {

    }

    public void VolumeChanged()
    {
        foreach(AudioSource _sound in _sounds)
        {
            _sound.volume = _volumeSlider.value;
        }
        _settings.SoundLevels = _volumeSlider.value;
    }

    public void PlayAgain()
    {
        _pauseButton.SetActive(true);
        GameManager.Instance.Restart();
    }

    public void GameOver(SideColor _winner)
    {
        _pauseButton.SetActive(false);

        if(_winner == SideColor.Both ||  _winner == SideColor.None)
        {
            _winnerText.SetText("DRAW");
        }
        else
        {
            _winnerText.SetText(_winner+" WINS");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
