using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    private Piece[,] grid;
    [SerializeField]
    private int _boardSize;
    public int BoardSize { get => _boardSize; }
    [SerializeField]
    private GameObject _blackPieces;
    [SerializeField]
    private GameObject _whitePieces;
    public static float Offset = 1.5f;

    private static BoardState _instance;
    public static BoardState Instance { get => _instance; }

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
        grid = new Piece[_boardSize, _boardSize];
        InitializeGrid();

    }

    public void InitializeGrid()
    {
        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = null;
            }
        }

        foreach (Transform child in _blackPieces.transform)
        {
            int x = (int)(child.localPosition.x / Offset);
            int y = (int)(child.localPosition.z / Offset);
            grid[x, y] = child.GetComponent<Piece>();
        }
        foreach (Transform child in _whitePieces.transform)
        {
            int x = (int)(child.localPosition.x / Offset);
            int y = (int)(child.localPosition.z / Offset);
            grid[x, y] = child.GetComponent<Piece>();
        }
    }

    public Piece GetField(int _width, int _length)
    {
        try
        {
            return grid[_width, _length];
        }
        catch
        {
            Debug.Log("Grid index out of bounds");
            return null;
        }
    }

    public void SetField(Piece _piece, int _widthNew, int _lengthNew)
    {
        grid[(int)(_piece.transform.localPosition.x/ BoardState.Offset), (int)(_piece.transform.localPosition.z / BoardState.Offset)] = null;
        grid[_widthNew, _lengthNew] = _piece;
    }

    public void ClearField(int _width, int _length)
    {
        grid[_width, _length] = null;
    }

    public bool IsInBorders(int _x, int _y)
    {
        if(_x >=0 && _x < _boardSize && _y >= 0 && _y < _boardSize)
        {
            return true;
        }

        return false;
    }

    public SideColor CalculateCheckState(int _xOld, int _yOld, int _xNew, int _yNew)
    {
        Piece _missplaced = grid[_xNew, _yNew];
        grid[_xNew, _yNew] = grid[_xOld, _yOld];
        grid[_xOld, _yOld] = null;

        SideColor _checkSide = CheckStateCalculator.CalculateCheck(grid);

        grid[_xOld, _yOld] = grid[_xNew, _yNew];
        grid[_xNew, _yNew] = _missplaced;

        return _checkSide;
    }

    public SideColor CheckIfGameOver()
    {
        return GameEndCalculator.CheckIfGameEnd(grid);
    }

    public void ResetPieces()
    {
        Piece _piece;
        Queue<Piece> _pieces = new Queue<Piece>();
        foreach (Transform child in _blackPieces.transform)
        {
            _piece = child.GetComponent<Piece>();
            if (_piece.WasPawn != null)
            {
                _pieces.Enqueue(_piece.WasPawn);
                _piece.WasPawn.gameObject.SetActive(true);
            }
            _pieces.Enqueue(_piece);
        }
        foreach (Transform child in _whitePieces.transform)
        {
            _piece = child.GetComponent<Piece>();
            if (_piece.WasPawn != null)
            {
                _pieces.Enqueue(_piece.WasPawn);
                _piece.WasPawn.gameObject.SetActive(true);
            }
            _pieces.Enqueue(_piece);
        }

        while (_pieces.Count > 0)
        {
            _piece = _pieces.Dequeue();
            if (_piece.WasPawn == null)
            {
                _piece.ResetPosition();
                _piece.HasMoved = false;
            }
            else
            {
                Destroy(_piece.gameObject);
            }
        }

        InitializeGrid();
    }

    public void PromotePawn(Pawn _promotingPawn, Piece _piece)
    {
        int x = (int)(_promotingPawn.transform.localPosition.x / Offset);
        int y = (int)(_promotingPawn.transform.localPosition.z / Offset);

        grid[x, y] = _piece;
        _piece.WasPawn = _promotingPawn;
        _piece.transform.position = _promotingPawn.transform.position;
        _promotingPawn.gameObject.SetActive(false);
    }

}
