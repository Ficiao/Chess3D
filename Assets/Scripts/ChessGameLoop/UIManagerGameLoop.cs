using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerGameLoop : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _settingsMenu;
    [SerializeField]
    private GameObject _gameOverMenu;
    [SerializeField]
    private GameObject _pawnPromotionMenu;
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
    [SerializeField]
    private Queen _whiteQueen;
    [SerializeField]
    private Queen _blackQueen;
    [SerializeField]
    private Bishop _whiteBishop;
    [SerializeField]
    private Bishop _blackBishop;
    [SerializeField]
    private Rook _whiteRook;
    [SerializeField]
    private Rook _blackRook;
    [SerializeField]
    private Knight _whiteKnight;
    [SerializeField]
    private Knight _blackKnight;
    private SideColor _pawnColor = SideColor.None;

    private static UIManagerGameLoop _instance;
    public static UIManagerGameLoop Instance { get => _instance; }

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
        _gameOverMenu.SetActive(false);
        GameManager.Instance.Restart();
    }

    public void GameOver(SideColor _winner)
    {
        _pauseButton.SetActive(false);
        _gameOverMenu.SetActive(true);
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

    public void PawnPromotionMenu(SideColor _color)
    {
        _pawnColor = _color;
        _pawnPromotionMenu.SetActive(true);
    }

    public void SelectedQueen()
    {
        if (_pawnColor == SideColor.Black)
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_blackQueen));
        }
        else
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_whiteQueen));
        }
        _pawnColor = SideColor.None;
        _pawnPromotionMenu.SetActive(false);
    }

    public void SelectedRook()
    {
        if (_pawnColor == SideColor.Black)
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_blackRook));
        }
        else
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_whiteRook));
        }
        _pawnColor = SideColor.None;
        _pawnPromotionMenu.SetActive(false);
    }

    public void SelectedBishop()
    {
        if (_pawnColor == SideColor.Black)
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_blackBishop));
        }
        else
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_whiteBishop));
        }
        _pawnColor = SideColor.None;
        _pawnPromotionMenu.SetActive(false);
    }

    public void SelectedKnight()
    {
        if (_pawnColor == SideColor.Black)
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_blackKnight));
        }
        else
        {
            GameManager.Instance.SelectedPromotion(Instantiate(_whiteKnight));
        }
        _pawnColor = SideColor.None;
        _pawnPromotionMenu.SetActive(false);
    }
}
