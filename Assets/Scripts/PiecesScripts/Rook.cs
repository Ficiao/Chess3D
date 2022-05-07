using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{

    public override void CreatePath()
    {
        PathCalculator.VerticalPath(this);
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        return CheckStateCalculator.SearchForKingVertical(_xPosition, _yPosition, PieceColor);
    }
}