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

        int _xSource = (int)(_caller.transform.localPosition.x / BoardState.Displacement);
        int _ySource = (int)(_caller.transform.localPosition.z / BoardState.Displacement);

        for (int j = 0; j < DiagonalLookup.GetLength(0); j++)
        {
            for (int i = 1; BoardState.Instance.IsInBorders(_xSource + i * _lookupTable[j, 0], _ySource + i * _lookupTable[j, 1]); i++)
            {
                _piece = BoardState.Instance.GetField(_xSource + i * _lookupTable[j, 0], _ySource + i * _lookupTable[j, 1]);
                if (_piece == null)
                {
                    _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                    _position.x = _caller.transform.localPosition.x + i * BoardState.Displacement * _lookupTable[j, 0];
                    _position.z = _caller.transform.localPosition.z + i * BoardState.Displacement * _lookupTable[j, 1];
                    _position.y = _path.transform.localPosition.y;

                    _path.transform.localPosition = _position;
                }
                else if (_piece.PieceColor != _caller.PieceColor)
                {
                    _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                    _path.GetComponent<PathPiece>().AssignPiece(_piece);
                    _position.x = _caller.transform.localPosition.x + i * BoardState.Displacement * _lookupTable[j, 0];
                    _position.z = _caller.transform.localPosition.z + i * BoardState.Displacement * _lookupTable[j, 1];
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

        int _xSource = (int)(_caller.transform.localPosition.x / BoardState.Displacement);
        int _ySource = (int)(_caller.transform.localPosition.z / BoardState.Displacement);

        if (BoardState.Instance.IsInBorders(_xSource + _xDirection, _ySource + _yDirection))
        {
            Piece _piece = BoardState.Instance.GetField(_xSource + _xDirection, _ySource + _yDirection);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = _caller.transform.localPosition.x + _xDirection * BoardState.Displacement;
                _position.z = _caller.transform.localPosition.z + _yDirection * BoardState.Displacement;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _path.GetComponent<PathPiece>().AssignPiece(_piece);
                _position.x = _caller.transform.localPosition.x + _xDirection * BoardState.Displacement;
                _position.z = _caller.transform.localPosition.z + _yDirection * BoardState.Displacement;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;

            }
        }
    }

}
