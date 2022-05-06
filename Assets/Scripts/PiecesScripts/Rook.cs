using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{


    public override void CreatePath()
    {
        GameObject[] _paths = ObjectPool.Instance.GetHighlightPaths(14);
        Vector3 _position=new Vector3();
        int index = 0;
        for(int i = 0; i < 8; i++)
        {
            if (i != (float)(transform.position.x / 1.5))
            {
                _position.x = transform.position.x;
                _position.y = i * 1.5f;
                _paths[index++].transform.position = _position;
            }

            if (i != (float)(transform.position.y / 1.5))
            {
                _position.x = i * 1.5f;
                _position.y = transform.position.y;
                _paths[index++].transform.position = _position;
            }

        }
    }

}