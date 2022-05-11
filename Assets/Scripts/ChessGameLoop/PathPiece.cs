using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChessMainLoop
{ 
public delegate void PathSelect(PathPiece piece);

public class PathPiece : MonoBehaviour
{
    public static event PathSelect PathSelect;
    [SerializeField]
    private string _name;
    public string Name { get => _name; }

    private Color _startColor;
    private Renderer _renderer;
    private Piece _assignedPiece = null;
    public Piece AssignedPiece { get => _assignedPiece; set => _assignedPiece = value; }
    private Piece _assignedCastle = null;
    public Piece AssignedCastle { get => _assignedCastle; set => _assignedCastle = value; }

    void OnEnable()
    {
        PieceController.PieceMoved += Disable;
    }
    
    void OnDisable()
    {
        PieceController.PieceMoved -= Disable;
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = Color.white;
    }

    private void OnMouseExit()
    {
        _renderer.material.color = _startColor;
    }

    private void OnMouseDown()
    {
        Selected();
    }

    private void Disable()
    {
        if (_assignedPiece != null)
        {
            _assignedPiece.AssignedAsEnemy = null;
            _assignedPiece = null;
        }
        else if (_assignedCastle != null)
        {
            _assignedCastle.AssignedAsCastle = null;
            _assignedCastle = null;
        }
        ObjectPool.Instance.RemoveHighlightPath(this);
    }

    public void AssignPiece(Piece _piece)
    {
        _assignedPiece = _piece;
        _assignedPiece.AssignedAsEnemy = this;
    }

    public void AssignCastle(Piece _piece)
    {
        _assignedCastle = _piece;
        _piece.AssignedAsCastle = this;
    }

    public void Selected()
    {
        _renderer.material.color = _startColor;
        PathSelect?.Invoke(this);
    }
}
}
