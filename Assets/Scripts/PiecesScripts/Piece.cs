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
    public Vector3 StartPosition { get => _startPosition; }
    private bool _active;
    public bool Active { get => _active; set { _active = false; _renderer.material.color = _startColor; } }
    private Color _startColor;
    private bool _moved;
    public bool HasMoved { get => _moved; set => _moved = true; }
    private PathPiece _assignedAsEnemy;
    public PathPiece AssignedAsEnemy { get => _assignedAsEnemy; set => _assignedAsEnemy = value; }

    public static event Selected Selected;

    public abstract void CreatePath();
    public abstract bool IsAttackingKing(int _xPosition, int _yPosition);

    private void Start()
    {
        _assignedAsEnemy = null;
        _startPosition = transform.position;
        _active = false;
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
        _moved = false;
    }

    private void OnMouseDown()
    {
        if (_active == false && GameManager.Instance.TurnPlayer == _pieceColor) 
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
    }

    private void OnMouseExit()
    {
        if ((_active == false && GameManager.Instance.TurnPlayer == _pieceColor) || _assignedAsEnemy)
        {
            _renderer.material.color = _startColor;
        }

    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Move(Vector3 _position)
    {
        _position.y = 0;
        BoardState.Instance.SetField(this, (int)(_position.x / BoardState.Displacement), (int)(_position.z / BoardState.Displacement));
        transform.localPosition = _position;
        _moved = true;
    }

}
