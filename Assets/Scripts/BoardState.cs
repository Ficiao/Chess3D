using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    private Piece[,] grid;
    [SerializeField]
    private int _width;
    public int Width { get => _width; }
    [SerializeField]
    private int _length;
    public int Length { get => _length; }
    [SerializeField]
    private GameObject _blackPieces;
    [SerializeField]
    private GameObject _whitePieces;
    public static float Displacement = 1.5f;

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
        grid = new Piece[_width, _length];
        foreach(Transform child in _blackPieces.transform)
        {
            int x = (int)(child.localPosition.x / Displacement);
            int y = (int)(child.localPosition.z / Displacement);
            grid[x, y] = child.GetComponent<Piece>();
        }
        foreach (Transform child in _whitePieces.transform)
        {
            int x = (int)(child.localPosition.x / Displacement);
            int y = (int)(child.localPosition.z / Displacement);
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
        grid[(int)(_piece.transform.localPosition.x/ BoardState.Displacement), (int)(_piece.transform.localPosition.z / BoardState.Displacement)] = null;
        grid[_widthNew, _lengthNew] = _piece;
    }

    public bool IsInBorders(int _x, int _y)
    {
        if(_x >=0 && _x < _width && _y >= 0 && _y < _length)
        {
            return true;
        }

        return false;
    }

    public SideColor CalculateCheckState(Vector3 _oldPosition, Vector3 _newPosition)
    {
        int _xOld = (int)(_oldPosition.x / BoardState.Displacement);
        int _yOld = (int)(_oldPosition.z / BoardState.Displacement);

        int _xNew = (int)(_newPosition.x / BoardState.Displacement);
        int _yNew = (int)(_newPosition.z / BoardState.Displacement);

        Piece _missplaced = grid[_xNew, _yNew];
        grid[_xNew, _yNew] = grid[_xOld, _yOld];
        grid[_xOld, _yOld] = null;

        SideColor _checkSide = CheckStateCalculator.CalculateCheck(grid);

        grid[_xOld, _yOld] = grid[_xNew, _yNew];
        grid[_xNew, _yNew] = _missplaced;

        return _checkSide;
    }
}
