using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChessReplay
{ 
public class BoardStateReplay : MonoBehaviour
{
    [SerializeField]
    private int _boardSize;
    public int BoardSize { get => _boardSize; }
    private Transform[,] grid;
    [SerializeField]
    private GameObject _blackPieces;
    [SerializeField]
    private GameObject _whitePieces;
    public static float Displacement = 1.5f;
    private Dictionary<int, GameObject> _killedDict;

    private static BoardStateReplay _instance;
    public static BoardStateReplay Instance { get => _instance; }

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

    public void InitializeGrid()
    {
        grid = new Transform[_boardSize, _boardSize];
        _killedDict = new Dictionary<int, GameObject>();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = null;
            }
        }

        foreach (Transform child in _blackPieces.transform)
        {
            int x = (int)(child.localPosition.x / Displacement);
            int y = (int)(child.localPosition.z / Displacement);
            grid[x, y] = child;
        }
        foreach (Transform child in _whitePieces.transform)
        {
            int x = (int)(child.localPosition.x / Displacement);
            int y = (int)(child.localPosition.z / Displacement);
            grid[x, y] = child;
        }
    }

    public void MovePiece(Vector2 _startPosition, Vector2 _endPosition, int _turnCount)
    {
        if (grid == null)
        {
            InitializeGrid();
        }

        if(grid[(int)_endPosition.x, (int)_endPosition.y] != null)
        {
            _killedDict.Add(_turnCount, grid[(int)_endPosition.x, (int)_endPosition.y].gameObject);
            grid[(int)_endPosition.x, (int)_endPosition.y].gameObject.SetActive(false);
        }

        grid[(int)_endPosition.x, (int)_endPosition.y] = grid[(int)_startPosition.x, (int)_startPosition.y];
        grid[(int)_startPosition.x, (int)_startPosition.y] = null;
        grid[(int)_endPosition.x, (int)_endPosition.y].localPosition = new Vector3(_endPosition.x * Displacement, 
            grid[(int)_endPosition.x, (int)_endPosition.y].localPosition.y, _endPosition.y * Displacement);
    }

    public void UndoPiece(Vector2 _startPosition, Vector2 _endPosition, int _turnCount)
    {
        MovePiece(_startPosition, _endPosition, _turnCount);

        if(_killedDict.TryGetValue(_turnCount, out GameObject _killed))
        {
            _killed.SetActive(true);
            grid[(int)_startPosition.x, (int)_startPosition.y] = _killed.transform;
        }
    }
}
}