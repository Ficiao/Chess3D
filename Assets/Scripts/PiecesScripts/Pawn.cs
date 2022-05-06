using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override void CreatePath()
    {
        if (PieceColor == SideColor.Black)
        {
            PathCalculator.PathOneSpot(this, 1, 0);
        }
        else
        {
            PathCalculator.PathOneSpot(this, -1, 0);
        }
    }

}
