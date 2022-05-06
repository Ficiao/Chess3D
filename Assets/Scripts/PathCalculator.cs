using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathCalculator
{
    public static void DiagonalPath(Piece _caller)
    {
        GameObject _path;
        Vector3 _position = new Vector3();
        Piece _piece;

        int _xSource = (int)(_caller.transform.localPosition.x / 1.5f);
        int _ySource = (int)(_caller.transform.localPosition.z / 1.5f);


        for (int i = 1; (_xSource + i) < BoardState.Instance.Width && (_ySource + i) < BoardState.Instance.Length; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource + i, _ySource + i);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z + i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z + i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
                break;
            }
            else
            {
                break;
            }
        }

        for (int i = 1; (_xSource + i) < BoardState.Instance.Width && (_ySource - i) >= 0; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource + i, _ySource - i);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z - i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z - i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
                break;
            }
            else
            {
                break;
            }
        }

        for (int i = 1; (_xSource - i) >= 0 && (_ySource - i) >= 0; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource - i, _ySource - i);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = -i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z - i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = -i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z - i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
                break;
            }
            else
            {
                break;
            }
        }

        for (int i = 1; (_xSource - i) >= 0 && (_ySource + i) < BoardState.Instance.Length; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource - i, _ySource + i);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = -i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z + i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = -i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z + i * 1.5f;
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

    public static void VerticalPath(Piece _caller)
    {
        GameObject _path;
        Vector3 _position = new Vector3();
        Piece _piece;

        int _xSource = (int)(_caller.transform.localPosition.x / 1.5f);
        int _ySource = (int)(_caller.transform.localPosition.z / 1.5f);


        for (int i = 1; (_xSource + i) < BoardState.Instance.Width; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource + i, _ySource);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
                break;
            }
            else
            {
                break;
            }
        }

        for (int i = 1; (_xSource - i) >= 0; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource - i, _ySource);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = -i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = -i * 1.5f + _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
                break;
            }
            else
            {
                break;
            }
        }

        for (int i = 1; (_ySource + i) < BoardState.Instance.Length; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource, _ySource + i);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z + i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z + i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
                break;
            }
            else
            {
                break;
            }
        }

        for (int i = 1; (_ySource - i) >= 0; i++)
        {
            _piece = BoardState.Instance.GetField(_xSource, _ySource - i);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z - i * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = _caller.transform.localPosition.x;
                _position.z = _caller.transform.localPosition.z - i * 1.5f;
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

    public static void PathOneSpot(Piece _caller, int _xDirection, int _yDirection)
    {
        GameObject _path;
        Vector3 _position = new Vector3();

        int _xSource = (int)(_caller.transform.localPosition.x / 1.5f);
        int _ySource = (int)(_caller.transform.localPosition.z / 1.5f);

        if (_xSource + _xDirection < BoardState.Instance.Length && _xSource + _xDirection >= 0 && _ySource + _yDirection < BoardState.Instance.Width && _ySource + _yDirection >= 0)
        {
            Piece _piece = BoardState.Instance.GetField((int)(_caller.transform.localPosition.x / 1.5f) + _xDirection, (int)(_caller.transform.localPosition.z / 1.5f) + _yDirection);
            if (_piece == null)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathYellow");
                _position.x = _caller.transform.localPosition.x + _xDirection * 1.5f;
                _position.z = _caller.transform.localPosition.z + _yDirection * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;
            }
            else if (_piece.PieceColor != _caller.PieceColor)
            {
                _path = ObjectPool.Instance.GetHighlightPath("HighlightPathRed");
                _position.x = _caller.transform.localPosition.x + _xDirection * 1.5f;
                _position.z = _caller.transform.localPosition.z + _yDirection * 1.5f;
                _position.y = _path.transform.localPosition.y;

                _path.transform.localPosition = _position;

            }
        }
    }
}
