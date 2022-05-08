using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerMainMenu : MonoBehaviour
{
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

    public void Play()
    {
        SceneManager.LoadScene("ChessGameLoop");
    }

    public void Replay()
    {

    }

    public void Setting()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
