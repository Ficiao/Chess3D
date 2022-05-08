using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private static readonly int[,] LookupMoves =
    {
       { 1, 1 },
       { 1, 0 },
       { 1, -1 },
       { 0, -1 },
       { -1, -1 },
       { -1, 0 },
       { -1, 1 },
       { 0, 1 }

    };

    [SerializeField]
    private List<Rook> _rooks;

    public override void CreatePath()
    {
        int _xPosition = (int)(transform.localPosition.x / BoardState.Displacement);
        int _yPosition = (int)(transform.localPosition.z / BoardState.Displacement);

        bool _allowed;
        for (int i = 0; i < LookupMoves.GetLength(0); i++)
        {
            _allowed = true;
            for (int j = 0; j < LookupMoves.GetLength(0); j++)
            {
                if (BoardState.Instance.IsInBorders(_xPosition + LookupMoves[i, 0] + LookupMoves[j, 0], _yPosition + LookupMoves[i, 1] + LookupMoves[j, 1]))
                {
                    if (BoardState.Instance.GetField(_xPosition + LookupMoves[i, 0] + LookupMoves[j, 0], _yPosition + LookupMoves[i, 1] + LookupMoves[j, 1]) is King
                        && BoardState.Instance.GetField(_xPosition + LookupMoves[i, 0] + LookupMoves[j, 0], _yPosition + LookupMoves[i, 1] + LookupMoves[j, 1]) != this)
                    {
                        _allowed = false;
                    }
                }
            }
            if (_allowed)
            {
                PathCalculator.PathOneSpot(this, LookupMoves[i, 0], LookupMoves[i, 1]);
            }
        }

        foreach(Piece _rook in _rooks)
        {
            if (_rook.HasMoved == false && HasMoved == false)
            {
                PathCalculator.CastleSpot(this, _rook);
            }
        }
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        return false;
    }

    public override bool CanMove(int _xPosition, int _yPosition)
    {
        for (int i = 0; i < LookupMoves.GetLength(0); i++)
        {
            if (GameEndCalculator.CanMoveToSpot(_xPosition, _yPosition, LookupMoves[i, 0], LookupMoves[i, 1], PieceColor))
            {
                return true;
            }
        }

        return false;
    }
}
