using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PieceMoved();

public class GameManager : MonoBehaviour
{
    private SideColor _turnPlayer;
    public SideColor TurnPlayer { get => _turnPlayer; }

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

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

    private void OnEnable()
    {
        PathPiece.PathSelect += ChangeTurn;
    }

    private void OnDisable()
    {
        PathPiece.PathSelect -= ChangeTurn;
    }

    private void Start()
    {
        _turnPlayer = SideColor.White;
    }

    private void ChangeTurn(PathPiece _piece)
    {
        _turnPlayer = _turnPlayer == SideColor.White ? SideColor.Black : SideColor.White;
    }

}
