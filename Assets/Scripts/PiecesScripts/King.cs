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

    public override void CreatePath()
    {
        for (int i = 0; i < LookupMoves.GetLength(0); i++)
        {
            PathCalculator.PathOneSpot(this, LookupMoves[i, 0], LookupMoves[i, 1]);
        }
    }

    public override bool IsAttackingKing(int _xPosition, int _yPosition)
    {
        return false;
    }
}
