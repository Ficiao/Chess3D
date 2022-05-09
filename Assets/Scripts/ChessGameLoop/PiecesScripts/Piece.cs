using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Selected(Piece self);
public enum SideColor
{
    White,
    Black,
    None,
    Both
}

public abstract class Piece : MonoBehaviour
{

    [SerializeField]
    private SideColor _pieceColor;
    public SideColor PieceColor { get => _pieceColor; }
    private Renderer _renderer;
    private Vector3 _startPosition;
    private bool _active = false;
    public bool Active { get => _active; set { _active = false; _renderer.material.color = _startColor; } }
    private Color _startColor;
    private bool _moved = false;
    public bool HasMoved { get => _moved; set => _moved = true; }
    private PathPiece _assignedAsEnemy = null;
    public PathPiece AssignedAsEnemy { get => _assignedAsEnemy; set => _assignedAsEnemy = value; }
    private PathPiece _assignedAsCastle = null;
    public PathPiece AssignedAsCastle { get => _assignedAsCastle; set { _assignedAsCastle = value; _renderer.material.color = _startColor; } }

    public static event Selected Selected;

    public abstract void CreatePath();
    public abstract bool IsAttackingKing(int _xPosition, int _yPosition);
    public abstract bool CanMove(int _xPosition, int _yPosition);

    private void Start()
    {
        _startPosition = transform.position;
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
    }

    private void OnMouseDown()
    {
        if (_active == false && GameManager.Instance.TurnPlayer == _pieceColor && _assignedAsCastle == false) 
        {
            _active = true;
            Selected?.Invoke(this);
            CreatePath();
            _renderer.material.color = Color.yellow;
        }
        else if (_assignedAsEnemy)
        {
            _assignedAsEnemy.Selected();
        }
        else if (_assignedAsCastle)
        {
            _assignedAsCastle.Selected();
        }
    }

    private void OnMouseEnter()
    {
        if (PieceController.Instance.AnyActive == false && GameManager.Instance.TurnPlayer == _pieceColor)
        {
            _renderer.material.color = Color.yellow;
        }
        else if (_assignedAsEnemy)
        {
            _renderer.material.color = Color.red;
        }
        else if (_assignedAsCastle)
        {
            _renderer.material.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        if ((_active == false && GameManager.Instance.TurnPlayer == _pieceColor) || _assignedAsEnemy || _assignedAsCastle)
        {
            _renderer.material.color = _startColor;
        }

    }

    public void Die()
    {
        BoardState.Instance.ClearField((int)(transform.localPosition.x / BoardState.Displacement), (int)(transform.localPosition.z / BoardState.Displacement));
        ObjectPool.Instance.AddPiece(this);
    }

    public void ResetPosition()
    {
        transform.position = _startPosition;
    }

    public virtual void Move(int _xPosition, int _yPosition)
    {
        MoveTracker.Instance.AddMove((int)(transform.position.x / BoardState.Displacement), (int)(transform.position.y / BoardState.Displacement), 
            _xPosition, _yPosition, GameManager.Instance.TurnCount);
        BoardState.Instance.SetField(this, _xPosition, _yPosition);
        Vector3 _position = transform.localPosition;
        _position.x = _xPosition * BoardState.Displacement;
        _position.z = _yPosition * BoardState.Displacement;
        _position.y = 0;
        transform.localPosition = _position;
        _moved = true;
    }

}
