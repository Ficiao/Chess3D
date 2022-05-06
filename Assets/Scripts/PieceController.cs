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

    private void PathSelected(Vector3 _position)
    {
        _activePiece.Move(_position);
        _activePiece.Active = false;
        _activePiece = null;
        PieceMoved?.Invoke();
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
