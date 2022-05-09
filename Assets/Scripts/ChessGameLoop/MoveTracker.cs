using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MoveTracker : MonoBehaviour
{
    private List<List<Vector2>> _moves;
    private static string[] _files =
    {
        "/Saves/save1.txt",
        "/Saves/save2.txt",
        "/Saves/save3.txt",
        "/Saves/save4.txt"
    };

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

    public void SaveGame(int _fileIndex)
    {
        string _json = "";
        for (int i = 0; i < _moves.Count; i++) 
        {
            var _myclass=new Serializator(_moves[i], i);
            _json =_json + "\n" + JsonUtility.ToJson(_myclass);            
        }

        File.WriteAllText(Application.dataPath + _files[_fileIndex], _json);


    }

    public class Serializator
    {
        public List<Vector2> MoveList;
        public int TurnOrder;

        public Serializator(List<Vector2> _moveList, int _turnOrder)
        {
            MoveList = _moveList;
            TurnOrder = _turnOrder;
        }
    }
}
