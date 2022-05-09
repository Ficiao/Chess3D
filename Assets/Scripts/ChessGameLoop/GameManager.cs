using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PieceMoved();

public class GameManager : MonoBehaviour
{
    private int _turnCount = 0;
    public int TurnCount { get => _turnCount; }
    private SideColor _turnPlayer;
    public SideColor TurnPlayer { get => _turnPlayer; }
    private SideColor _checkedSide;
    public SideColor CheckedSide { get => _checkedSide; set => _checkedSide = value == SideColor.Both ? _turnPlayer == SideColor.White ? SideColor.Black : SideColor.White : value; }
    private Pawn _passantable = null;
    public Pawn Passantable { get => _passantable; set => _passantable = value; }
    [SerializeField]
    private CameraControl _camera;

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
        _turnCount++;
    }

    public void GameEnd(SideColor _winner)
    {
        UIManager.Instance.GameOver(_winner);
        _camera.enabled = false;
        _turnPlayer = SideColor.None;
    }

    public void Restart()
    {
        ObjectPool.Instance.ResetPieces();
        BoardState.Instance.ResetPieces();
        _camera.enabled = true;
        MoveTracker.Instance.ResetMoves();
        _turnCount = 0;
        _turnPlayer = SideColor.White;
    }
}
