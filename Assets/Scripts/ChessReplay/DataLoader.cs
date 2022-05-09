using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DataLoader
{
    private static string[] _files =
    {
        "/Saves/save1.txt",
        "/Saves/save2.txt",
        "/Saves/save3.txt",
        "/Saves/save4.txt"
    };

    public static List<List<Vector2>> LoadData(int _fileIndex)
    {
        List<List<Vector2>> _moves = new List<List<Vector2>>();

        if (File.Exists(Application.dataPath + _files[_fileIndex]))
        {
            string[] _json = File.ReadAllText(Application.dataPath + _files[_fileIndex]).Split("\n");
            Serializator _moveInstance;

            foreach (string _turn in _json)
            {
                if (_turn.Length == 0)
                {
                    continue;
                }
                _moveInstance = JsonUtility.FromJson<Serializator>(_turn);
                _moves.Add(_moveInstance.MoveList);
            }
        }
        
        return _moves;
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
