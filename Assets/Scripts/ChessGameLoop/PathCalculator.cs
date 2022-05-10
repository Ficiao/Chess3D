using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathCalculator
{
    private static readonly int[,] DiagonalLookup =
    {
       { 1, 1 },
       { 1, -1 },
       { -1, 1 },
       { -1, -1 }
    };

    private static readonly int[,] VerticalLookup =
    {
       { 1, 0 },
       { -1, 0 },
       { 0, 1 },
       { 0, -1 }
    };

    public static void DiagonalPath(Piece _caller)
    {
        CalculatePath(_caller, DiagonalLookup);
    }

    public static void VerticalPath(Piece _caller)
    {
        CalculatePath(_caller, VerticalLookup);
    }

    private static void CalculatePath(Piece _caller, int[,] _lookupTable)
    {
        GameObject _path;
        Vector3 _position = new Vector3();
        Piece _piece;

        int _xSource = (int)(_caller.transform.localPosition.x / BoardState.Offset);
        int _ySource = (int)(_caller.transform.localPosition.z / BoardState.Offset);

        for (int j = 0; j < DiagonalLookup.GetLength(0); j++)
        {
            for (int i = 1; BoardState.Instance.IsInBorders(_xSource + i * _lookupTable[j, 0], _ySource + i * _lookupTable[j, 1]); i++)
            {
                SideColor _checkSide = BoardState.Instance.CalculateCheckState(_xSource, _ySource, _xSource + i * _lookupTable[j, 0], _ySource + i * _lookupTable[j, 1]);
                _piece = BoardState.Instance.GetField(_xSource + i * _lookupTable[j, 0], _ySource + i * _lookupTable[j, 1]);

                if (_piece == null)
                {
                    if (_checkSide == _caller.PieceColor || _checkSide == SideColor.Both)
                    {
                        continue;
                    }

                    _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                    _position.x = _caller.transform.localPosition.x + i * BoardState.Offset * _lookupTable[j, 0];
                    _position.z = _caller.transform.localPosition.z + i * BoardState.Offset * _lookupTable[j, 1];
                    _position.y = _path.transform.localPosition.y;

                    _path.transform.localPosition = _position;
                }
                else if (_piece.PieceColor != _caller.PieceColor)
                {
                    if (_checkSide == _caller.PieceColor || _checkSide == SideColor.Both)
                    {
                        break;
                    }

                    _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                    _path.GetComponent<PathPiece>().AssignPiece(_piece);
                    _position.x = _caller.transform.localPosition.x + i * BoardState.Offset * _lookupTable[j, 0];
                    _position.z = _caller.transform.localPosition.z + i * BoardState.Offset * _lookupTable[j, 1];
                    _position.y = _path.transform.localPosition.y;

                    _path.transform.localPosition = _position;
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }

    public static void PathOneSpot(Piece _caller, int _xDirection, int _yDirection)
    {
        GameObject _path;
        Vector3 _position = new Vector3();

        int _xSource = (int)(_caller.transform.localPosition.x / BoardState.Offset);
        int _ySource = (int)(_caller.transform.localPosition.z / BoardState.Offset);

        if (BoardState.Instance.IsInBorders(_xSource + _xDirection, _ySource + _yDirection))
        {
            SideColor _checkSide = BoardState.Instance.CalculateCheckState(_xSource, _ySource, _xSource + _xDirection, _ySource + _yDirection);
            Piece _piece = BoardState.Instance.GetField(_xSource + _xDirection, _ySource + _yDirection);

            if (_piece == null)
            {
                if (_checkSide == _caller.PieceColor || _checkSide == SideColor.Both)
                {
                    return;
                }
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = _caller.transform.localPosition.x + _xDirection * BoardState.Offset;
                _position.z = _caller.transform.localPosition.z + _yDirection * BoardState.Offset;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                if (_checkSide == _caller.PieceColor || _checkSide == SideColor.Both)
                {
                    return;
                }
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _path.GetComponent<PathPiece>().AssignPiece(_piece);
                _position.x = _caller.transform.localPosition.x + _xDirection * BoardState.Offset;
                _position.z = _caller.transform.localPosition.z + _yDirection * BoardState.Offset;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;

            }
        }
    }

    public static void PassantSpot(Piece _target, int _xPosition, int _yPosition)
    {
        GameObject _path;
        Vector3 _position = new Vector3();

        _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
        _path.GetComponent<PathPiece>().AssignPiece(_target);
        _position.x = _xPosition * BoardState.Offset; 
        _position.z = _yPosition * BoardState.Offset; 
        _position.y = _path.transform.localPosition.y;

        _path.transform.localPosition = _position;
    }

    public static void CastleSpot(Piece _caller, Piece _target)
    {
        if (GameManager.Instance.CheckedSide == _caller.PieceColor)
        {
            return;
        }

        int _xCaller = (int)(_caller.transform.localPosition.x / BoardState.Offset);
        int _yCaller = (int)(_caller.transform.localPosition.z / BoardState.Offset);
        int _xTarget = (int)(_target.transform.localPosition.x / BoardState.Offset);
        int _yTarget = (int)(_target.transform.localPosition.z / BoardState.Offset);

        _yCaller+= _yTarget > _yCaller ? 1 : -1;
        while (_yCaller != _yTarget)
        {
            if (BoardState.Instance.GetField(_xCaller, _yCaller) != null)
            {
                return;
            }
            _yCaller += _yTarget > _yCaller ? 1 : -1;
        }

        _yCaller = (int)(_caller.transform.localPosition.z / BoardState.Offset);
        int _yMedian = (int)Mathf.Ceil((_yCaller + _yTarget) / 2f);

        if(BoardState.Instance.CalculateCheckState(_xCaller, _yCaller, _xCaller, _yMedian) == _caller.PieceColor && _caller is King)
        {
            return;
        }
        else if (BoardState.Instance.CalculateCheckState(_xTarget, _yTarget, _xTarget, _yMedian) == _caller.PieceColor && _target is King)
        {
            return;
        }

        Vector3 _position = new Vector3();
        PathPiece _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow").GetComponent<PathPiece>();
        _path.AssignCastle(_target);
        _position.x = _xTarget * BoardState.Offset;
        _position.z = _yTarget * BoardState.Offset;
        _position.y = _path.transform.localPosition.y;

        _path.transform.localPosition = _position;
    }

}
