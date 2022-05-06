using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    private Piece[,] grid;
    [SerializeField]
    private int _width;
    public int Width { get => _width; }
    [SerializeField]
    private int _length;
    public int Length { get => _length; }
    [SerializeField]
    private GameObject _blackPieces;
    [SerializeField]
    private GameObject _whitePieces;

    private static BoardState _instance;
    public static BoardState Instance { get => _instance; }

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
        grid = new Piece[_width, _length];
        foreach(Transform child in _blackPieces.transform)
        {
            int x = (int)(child.localPosition.x / 1.5f);
            int y = (int)(child.localPosition.z / 1.5f);
            grid[x, y] = child.GetComponent<Piece>();
        }
        foreach (Transform child in _whitePieces.transform)
        {
            int x = (int)(child.localPosition.x / 1.5f);
            int y = (int)(child.localPosition.z / 1.5f);
            grid[x, y] = child.GetComponent<Piece>();
        }
    }

    public Piece GetField(int _width, int _length)
    {
        return grid[_width, _length];
    }

    public void SetField(Piece _piece, int _widthNew, int _lengthNew)
    {
        grid[(int)(_piece.transform.localPosition.x/1.5f), (int)(_piece.transform.localPosition.z / 1.5f)] = null;
        grid[_widthNew, _lengthNew] = _piece;
    }
}
