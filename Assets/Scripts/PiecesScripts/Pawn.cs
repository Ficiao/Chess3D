using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{

    public override void CreatePath()
    {
        int _xSource = (int)(transform.localPosition.x / BoardState.Displacement);
        int _ySource = (int)(transform.localPosition.z / BoardState.Displacement);

        int _direction = PieceColor == SideColor.Black ? 1 : -1;

        if (BoardState.Instance.IsInBorders(_xSource + _direction, _ySource + 1))
        {
            if (BoardState.Instance.GetField(_xSource + _direction, _ySource + 1) != null ? BoardState.Instance.GetField(_xSource + _direction, _ySource + 1).PieceColor != PieceColor : false)
            {
                PathCalculator.PathOneSpot(this, _direction, 1);
            }
        }

        if (BoardState.Instance.IsInBorders(_xSource + _direction, _ySource - 1))
        {
            if (BoardState.Instance.GetField(_xSource + _direction, _ySource - 1) != null ? BoardState.Instance.GetField(_xSource + _direction, _ySource - 1).PieceColor != PieceColor : false)
            {
                PathCalculator.PathOneSpot(this, _direction, -1);
            }
        }

        if (BoardState.Instance.GetField(_xSource + _direction, _ySource) != null) 
        {
            return;
        }

        PathCalculator.PathOneSpot(this, _direction, 0);

        if (HasMoved == false && BoardState.Instance.GetField(_xSource + _direction * 2, _ySource) == null)
        {
            PathCalculator.PathOneSpot(this, _direction * 2, 0);
        }
       
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        int _direction = PieceColor == SideColor.Black ? 1 : -1;

        if (BoardState.Instance.IsInBorders(_xPosition + _direction, _yPosition + 1))
        {
            if (CheckStateCalculator.KingAtLocation(_xPosition, _yPosition, _direction, 1, PieceColor))
            {
                return true;
            }
        }

        if (BoardState.Instance.IsInBorders(_xPosition + _direction, _yPosition - 1))
        {
            if (CheckStateCalculator.KingAtLocation(_xPosition, _yPosition, _direction, -1, PieceColor))
            {
                return true;
            }
        }

        return false;
    }
}
