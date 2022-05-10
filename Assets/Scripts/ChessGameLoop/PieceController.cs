using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    private Piece _activePiece;
    public bool AnyActive { get => _activePiece != null; }
    [SerializeField]
    private Camera _camera;

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
        Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.TryGetComponent<PathPiece>(out PathPiece _path) || _hit.transform.TryGetComponent<Piece>(out Piece _piece))
            {
                return;
            }
        }

        PieceMoved?.Invoke();
        _activePiece.Active = false;
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
        Piece _assignedEnemy = _path.AssignedPiece;
        Piece _assignedCastle = _path.AssignedCastle;
        PieceMoved?.Invoke();
        StartCoroutine(PieceMover(_path, _assignedEnemy, _assignedCastle));
        _activePiece.Active = false;
    }

    private IEnumerator PieceMover(PathPiece _path, Piece _assignedEnemy, Piece _assignedCastle)
    {
        int _xPiece = (int)(_activePiece.transform.localPosition.x / BoardState.Offset);
        int _yPiece = (int)(_activePiece.transform.localPosition.z / BoardState.Offset);
        int _xPath = (int)(_path.transform.localPosition.x / BoardState.Offset);
        int _yPath = (int)(_path.transform.localPosition.z / BoardState.Offset);

        Vector3 _targetPosition=new Vector3();

        if (_assignedCastle == false)
        {
            SideColor _checked = BoardState.Instance.CalculateCheckState(_xPiece, _yPiece, _xPath, _yPath);
            GameManager.Instance.CheckedSide = _checked;

            _activePiece.Move(_xPath, _yPath);

            _targetPosition.x = _xPath * BoardState.Offset;
            _targetPosition.y = _activePiece.transform.localPosition.y;
            _targetPosition.z = _yPath * BoardState.Offset;
            AnimationManager.Instance.MovePiece(_activePiece, _targetPosition, _assignedEnemy);
            while (AnimationManager.Instance.Active == true)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            int _yMedian = (int)Mathf.Ceil((_yPiece + _yPath) / 2f);
            SideColor _checked;

            if (_activePiece is King)
            {
                _activePiece.Move(_xPath, _yMedian);
                _targetPosition.x = _xPath * BoardState.Offset;
                _targetPosition.y = _activePiece.transform.localPosition.y;
                _targetPosition.z = _yMedian * BoardState.Offset;
                AnimationManager.Instance.MovePiece(_activePiece, _targetPosition, null);
                while (AnimationManager.Instance.Active == true)
                {
                    yield return new WaitForSeconds(0.01f);
                }

                _checked = BoardState.Instance.CalculateCheckState(_xPath, _yPath, _xPath, _yMedian > _yPiece ? _yMedian - 1 : _yMedian + 1);

                _assignedCastle.Move(_xPath, _yMedian > _yPiece ? _yMedian - 1 : _yMedian + 1);
                _targetPosition.x = _xPath * BoardState.Offset;
                _targetPosition.y = _activePiece.transform.localPosition.y;
                _targetPosition.z = (_yMedian > _yPiece ? _yMedian - 1 : _yMedian + 1) * BoardState.Offset;
                AnimationManager.Instance.MovePiece(_assignedCastle, _targetPosition, null);
                while (AnimationManager.Instance.Active == true)
                {
                    yield return new WaitForSeconds(0.01f);
                }

            }
            else
            {
                _activePiece.Move(_xPath, _yMedian > _yPiece ? _yMedian + 1 : _yMedian - 1);
                _targetPosition.x = _xPath * BoardState.Offset;
                _targetPosition.y = _activePiece.transform.localPosition.y;
                _targetPosition.z = (_yMedian > _yPiece ? _yMedian + 1 : _yMedian - 1) * BoardState.Offset;
                AnimationManager.Instance.MovePiece(_activePiece, _targetPosition, null);
                while (AnimationManager.Instance.Active == true)
                {
                    yield return new WaitForSeconds(0.01f);
                }

                _checked = BoardState.Instance.CalculateCheckState(_xPath, _yPath, _xPath, _yMedian);
                _assignedCastle.Move(_xPath, _yMedian);

                _targetPosition.x = _xPath * BoardState.Offset;
                _targetPosition.y = _activePiece.transform.localPosition.y;
                _targetPosition.z = _yMedian * BoardState.Offset;
                AnimationManager.Instance.MovePiece(_assignedCastle, _targetPosition, null);
                while (AnimationManager.Instance.Active == true)
                {
                    yield return new WaitForSeconds(0.01f);
                }
            }

            GameManager.Instance.CheckedSide = _checked;
            _path.AssignedCastle.AssignedAsCastle = null;
            GameManager.Instance.Passantable = null;
        }
        _activePiece = null;
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
