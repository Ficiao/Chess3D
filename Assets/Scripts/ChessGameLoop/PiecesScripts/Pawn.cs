using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    [SerializeField]
    private GameObject _queenPrefab;

    public override void CreatePath()
    {
        int _xSource = (int)(transform.localPosition.x / BoardState.Displacement);
        int _ySource = (int)(transform.localPosition.z / BoardState.Displacement);

        int _direction = PieceColor == SideColor.Black ? 1 : -1;

        if (BoardState.Instance.IsInBorders(_xSource + _direction, _ySource + 1) == true)
        {
            if (BoardState.Instance.GetField(_xSource + _direction, _ySource + 1) != null ? BoardState.Instance.GetField(_xSource + _direction, _ySource + 1).PieceColor != PieceColor : false)
            {
                PathCalculator.PathOneSpot(this, _direction, 1);
            }
        }

        if (BoardState.Instance.IsInBorders(_xSource + _direction, _ySource - 1) == true)
        {
            if (BoardState.Instance.GetField(_xSource + _direction, _ySource - 1) != null ? BoardState.Instance.GetField(_xSource + _direction, _ySource - 1).PieceColor != PieceColor : false)
            {
                PathCalculator.PathOneSpot(this, _direction, -1);
            }
        }

        if (BoardState.Instance.IsInBorders(_xSource, _ySource + 1) == true)
        {
            if (BoardState.Instance.GetField(_xSource, _ySource + 1) != null ? BoardState.Instance.GetField(_xSource, _ySource + 1) == GameManager.Instance.Passantable : false)
            {
                PathCalculator.PassantSpot(BoardState.Instance.GetField(_xSource, _ySource + 1), _xSource + _direction, _ySource + 1);
            }
        }

        if (BoardState.Instance.IsInBorders(_xSource, _ySource - 1) == true)
        {
            if (BoardState.Instance.GetField(_xSource, _ySource - 1) != null ? BoardState.Instance.GetField(_xSource, _ySource - 1) == GameManager.Instance.Passantable : false)
            {
                if (BoardState.Instance.GetField(_xSource, _ySource - 1) is Pawn ? GameManager.Instance.Passantable : false)
                    PathCalculator.PassantSpot(BoardState.Instance.GetField(_xSource, _ySource - 1), _xSource + _direction, _ySource - 1);
            }
        }


        if (BoardState.Instance.IsInBorders(_xSource + _direction, _ySource)== false)
        {
            return;
        }

        if (BoardState.Instance.GetField(_xSource + _direction, _ySource) != null) 
        {
            return;
        }

        PathCalculator.PathOneSpot(this, _direction, 0);

        if (BoardState.Instance.IsInBorders(_xSource + _direction * 2, _ySource) == false)
        {
            return;
        }

        if (HasMoved == false && BoardState.Instance.GetField(_xSource + _direction * 2, _ySource) == null)
        {
            PathCalculator.PathOneSpot(this, _direction * 2, 0);
        }
       
    }

    public override void Move(int _xPosition, int _yPosition)
    {
        int _xPiece = (int)(transform.localPosition.x / BoardState.Displacement);

        if (Mathf.Abs(_xPiece - _xPosition) == 2)
        {
            GameManager.Instance.Passantable = this;
        }

        if (_xPosition == 0 || _xPosition == BoardState.Instance.BoardSize - 1)
        {
            GameManager.Instance.PawnPromoting(this);
        }

        base.Move(_xPosition, _yPosition);        
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        int _direction = PieceColor == SideColor.Black ? 1 : -1;

        if (CheckStateCalculator.KingAtLocation(_xPosition, _yPosition, _direction, 1, PieceColor))
        {
            return true;            
        }

        if (CheckStateCalculator.KingAtLocation(_xPosition, _yPosition, _direction, -1, PieceColor))
        {
            return true;
        }        

        return false;
    }

    public override bool CanMove(int _xPosition, int _yPosition)
    {
        int _direction = PieceColor == SideColor.Black ? 1 : -1;

        if (BoardState.Instance.IsInBorders(_xPosition + _direction, _yPosition + 1))
        {
            if (BoardState.Instance.GetField(_xPosition + _direction, _yPosition + 1) != null ?
                BoardState.Instance.GetField(_xPosition + _direction, _yPosition + 1).PieceColor != PieceColor : false)
            {
                if (GameEndCalculator.CanMoveToSpot(_xPosition, _yPosition, _direction, 1, PieceColor))
                {
                    return true;
                }
            }
        }

        if (BoardState.Instance.IsInBorders(_xPosition + _direction, _yPosition - 1))
        {
            if (BoardState.Instance.GetField(_xPosition + _direction, _yPosition - 1) != null ?
            BoardState.Instance.GetField(_xPosition + _direction, _yPosition - 1).PieceColor != PieceColor : false)
            {
                if (GameEndCalculator.CanMoveToSpot(_xPosition, _yPosition, _direction, -1, PieceColor))
                {
                    return true;
                }
            }
        }

        if (BoardState.Instance.IsInBorders(_xPosition + _direction, _yPosition) == false)
        {
            return false;
        }

        if (BoardState.Instance.GetField(_xPosition + _direction, _yPosition) != null)
        {
            return false;
        }

        if(GameEndCalculator.CanMoveToSpot(_xPosition, _yPosition, _direction, 0, PieceColor))
        {
            return true;
        }

        if (BoardState.Instance.IsInBorders(_xPosition + _direction, _yPosition) == false)
        {
            return false;
        }

        if (BoardState.Instance.GetField(_xPosition + _direction * 2, _yPosition) != null)
        {
            return false;
        }

        if (GameEndCalculator.CanMoveToSpot(_xPosition, _yPosition, _direction * 2, 0, PieceColor))
        {
            return true;
        }

        return false;
    }
}
