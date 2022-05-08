using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    private Piece _activePiece;
    public bool AnyActive { get => _activePiece != null; }
    [SerializeField]
    private new Camera camera;

    public static event PieceMoved PieceMoved;

    private static PieceController _instance;
    public static PieceController Instance { get => _instance; }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) == false || AnyActive == false)
        {
            return;
        }

        RaycastHit _hit;
        Ray _ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.TryGetComponent<PathPiece>(out PathPiece _path) || _hit.transform.TryGetComponent<Piece>(out Piece _piece))
            {
                return;
            }
        }

        _activePiece.Active = false;
        PieceMoved?.Invoke();
        _activePiece = null;


    }

    void OnEnable()
    {
        _activePiece = null;
        Piece.Selected += PieceSelected;
        PathPiece.PathSelect += PathSelected;
    }

    void OnDisable()
    {
        Piece.Selected -= PieceSelected;
        PathPiece.PathSelect -= PathSelected;
    }

    private void PathSelected(PathPiece _path)
    {
        int _xPiece = (int)(_activePiece.transform.localPosition.x / BoardState.Displacement);
        int _yPiece = (int)(_activePiece.transform.localPosition.z / BoardState.Displacement);
        int _xPath = (int)(_path.transform.localPosition.x / BoardState.Displacement);
        int _yPath = (int)(_path.transform.localPosition.z / BoardState.Displacement);

        if (_path.AssignedCastle == false)
        {
            SideColor _checked = BoardState.Instance.CalculateCheckState(_xPiece, _yPiece, _xPath, _yPath);
            GameManager.Instance.CheckedSide = _checked;
            if (_path.AssignedPiece != null)
            {
                _path.AssignedPiece.Die();
            }
            _activePiece.Move(_xPath, _yPath);
        }
        else
        {
            int _yMedian = (int)Mathf.Ceil((_yPiece + _yPath) / 2f);
            SideColor _checked;

            if (_activePiece is King)
            {
                _activePiece.Move(_xPath, _yMedian);
                _checked = BoardState.Instance.CalculateCheckState(_xPath, _yPath, _xPath, _yMedian > _yPiece ? _yMedian - 1 : _yMedian + 1);
                _path.AssignedCastle.Move(_xPath, _yMedian > _yPiece ? _yMedian - 1 : _yMedian + 1);
            }
            else
            {
                _activePiece.Move(_xPath, _yMedian > _yPiece ? _yMedian + 1 : _yMedian - 1);
                _checked = BoardState.Instance.CalculateCheckState(_xPath, _yPath, _xPath, _yMedian);
                _path.AssignedCastle.Move(_xPath, _yMedian);
            }

            GameManager.Instance.CheckedSide = _checked;
            _path.AssignedCastle.AssignedAsCastle = null;
        }

        GameManager.Instance.Passantable = null;
        _activePiece.Active = false;
        _activePiece = null;
        PieceMoved?.Invoke();
        GameManager.Instance.ChangeTurn();
    }

    private void PieceSelected(Piece _piece)
    {
        if (_activePiece)
        {
            _activePiece.Active = false;
            PieceMoved?.Invoke();
        }

        _activePiece = _piece;
    }
}
