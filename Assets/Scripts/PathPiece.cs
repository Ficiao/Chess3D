using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PathSelect(PathPiece piece);

public class PathPiece : MonoBehaviour
{
    public static event PathSelect PathSelect;
    [SerializeField]
    private string _name;
    public string Name { get => _name; }

    private Color _startColor;
    private Renderer _renderer;
    private Piece _assignedPiece;
    public Piece AssignedPiece { get => _assignedPiece; set => _assignedPiece = value; }

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
        ObjectPool.Instance.RemoveHighlightPath(this);
        if (_assignedPiece != null)
        {
            _assignedPiece.AssignedAsEnemy = null;
            _assignedPiece = null;
        }
    }

    public void AssignPiece(Piece _piece)
    {
        _assignedPiece = _piece;
        _assignedPiece.AssignedAsEnemy = this;
    }

    public void Selected()
    {
        _renderer.material.color = _startColor;
        PathSelect?.Invoke(this);
    }
}
