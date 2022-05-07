using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void CreatePath()
    {
        PathCalculator.DiagonalPath(this);
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        return CheckStateCalculator.SearchForKingDiagonal(_xPosition, _yPosition, PieceColor);
    }
}
