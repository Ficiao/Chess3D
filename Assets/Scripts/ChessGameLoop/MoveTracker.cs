using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTracker : MonoBehaviour
{
    private List<List<Vector2>> _moves;

    private static MoveTracker _instance;
    public static MoveTracker Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _moves = new List<List<Vector2>>();
    }

    public void AddMove(int _xOld, int _yOld, int _xNew, int _yNew, int _moveOrder)
    {
        if (_moves.Count <= _moveOrder)
        {
            _moves.Add(new List<Vector2>());
        }

        _moves[_moveOrder].Add(new Vector2(_xOld, _yOld));
        _moves[_moveOrder].Add(new Vector2(_xNew, _yNew));
    }

    public List<List<Vector2>> Moves()
    {
        return _moves;
    }

    public void ResetMoves()
    {
        _moves = new List<List<Vector2>>();
    }
}
