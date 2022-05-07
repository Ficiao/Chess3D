using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void CreatePath()
    {
        PathCalculator.DiagonalPath(this);
        PathCalculator.VerticalPath(this);
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        return CheckStateCalculator.SearchForKingVertical(_xPosition, _yPosition, PieceColor) || CheckStateCalculator.SearchForKingDiagonal(_xPosition, _yPosition, PieceColor);
    }
}
