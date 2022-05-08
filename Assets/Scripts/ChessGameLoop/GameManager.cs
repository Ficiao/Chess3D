using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PieceMoved();

public class GameManager : MonoBehaviour
{
    private SideColor _turnPlayer;
    public SideColor TurnPlayer { get => _turnPlayer; }
    private SideColor _checkedSide;
    public SideColor CheckedSide { get => _checkedSide; set => _checkedSide = value == SideColor.Both ? _turnPlayer == SideColor.White ? SideColor.Black : SideColor.White : value; }
    private Pawn _passantable = null;
    public Pawn Passantable { get => _passantable; set => _passantable = value; }

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

    private void Start()
    {
        _turnPlayer = SideColor.White;
        _checkedSide = SideColor.None;
    }

    public void ChangeTurn()
    {
        _turnPlayer = _turnPlayer == SideColor.White ? SideColor.Black : SideColor.White;
        SideColor _winner = BoardState.Instance.CheckIfGameOver();
        if (_winner != SideColor.None)
        {
            GameEnd(_winner);
        }
    }

    public void GameEnd(SideColor _winner)
    {
        Debug.Log("Check mate "+_winner);
    }
}
