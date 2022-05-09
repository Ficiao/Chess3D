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
    public SideColor TurnPlayer { get => _turnPlayer; set => _turnPlayer = value; }
    private SideColor _checkedSide;
    public SideColor CheckedSide { get => _checkedSide; set => _checkedSide = value == SideColor.Both ? _turnPlayer == SideColor.White ? SideColor.Black : SideColor.White : value; }
    private Pawn _passantable = null;
    public Pawn Passantable { get => _passantable; set => _passantable = value; }
    private Pawn _promotingPawn = null;
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
        if (_turnPlayer == SideColor.White)
        {
            _turnPlayer = SideColor.Black;
        }
        else if (_turnPlayer == SideColor.Black)
        {
            _turnPlayer = SideColor.White;
        }
        SideColor _winner = BoardState.Instance.CheckIfGameOver();
        if (_winner != SideColor.None)
        {
            GameEnd(_winner);
        }
        _turnCount++;
    }

    public void GameEnd(SideColor _winner)
    {
        UIManagerGameLoop.Instance.GameOver(_winner);
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
        _checkedSide = SideColor.None;
        _passantable = null;
    }

    public void PawnPromoting(Pawn _pawn)
    {
        _promotingPawn = _pawn;
        UIManagerGameLoop.Instance.PawnPromotionMenu(_pawn.PieceColor);
        _camera.enabled = false;
    }

    public void SelectedPromotion(Piece _piece)
    {
        _camera.enabled = true;
        _piece.transform.parent = _promotingPawn.transform.parent;
        _piece.transform.localScale = _promotingPawn.transform.localScale;
        BoardState.Instance.PromotePawn(_promotingPawn, _piece);
        SideColor _winner = BoardState.Instance.CheckIfGameOver();
        if (_winner != SideColor.None)
        {
            GameEnd(_winner);
        }
        _promotingPawn = null;
    }
}
