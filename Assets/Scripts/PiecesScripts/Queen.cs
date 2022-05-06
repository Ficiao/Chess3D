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

}
