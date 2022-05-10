using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
    private bool _hasMoved = false;
    public bool HasMoved { get => _hasMoved; set => _hasMoved = value; }
    private PathPiece _assignedAsEnemy = null;
    public PathPiece AssignedAsEnemy { get => _assignedAsEnemy; set => _assignedAsEnemy = value; }
    private PathPiece _assignedAsCastle = null;
    public PathPiece AssignedAsCastle { get => _assignedAsCastle; set { _assignedAsCastle = value; _renderer.material.color = _startColor; } }
    private Pawn _wasPawn = null;
    public Pawn WasPawn { get => _wasPawn; set => _wasPawn = value; }
    private Animator _animator;
    public Animator Animator { get => _animator; }

    public static event Selected Selected;

    public abstract void CreatePath();
    public abstract bool IsAttackingKing(int _xPosition, int _yPosition);
    public abstract bool CanMove(int _xPosition, int _yPosition);

    private void Start()
    {
        GetComponent<Animator>().runtimeAnimatorController = AnimationManager.Instance.Assign(this);
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
        if ((_active == false) || _assignedAsEnemy || _assignedAsCastle)
        {
            _renderer.material.color = _startColor;
        }

    }

    public void Die()
    {
        if (BoardState.Instance.GetField((int)(transform.localPosition.x / BoardState.Offset), (int)(transform.localPosition.z / BoardState.Offset)) == this)
        {
            BoardState.Instance.ClearField((int)(transform.localPosition.x / BoardState.Offset), (int)(transform.localPosition.z / BoardState.Offset));
        }
        ObjectPool.Instance.AddPiece(this);
    }

    public void ResetPosition()
    {
        transform.position = _startPosition;
        _renderer.material.color = _startColor;
    }

    public virtual void Move(int _xPosition, int _yPosition)
    {
        int _xSelf = (int)(transform.localPosition.x / BoardState.Offset);
        int _ySelf = (int)(transform.localPosition.z / BoardState.Offset);

        MoveTracker.Instance.AddMove(_xSelf, _ySelf, _xPosition, _yPosition, GameManager.Instance.TurnCount);

        if(this is Pawn && GameManager.Instance.Passantable)
        {
            int _direction = PieceColor == SideColor.Black ? 1 : -1;

            if (_yPosition * BoardState.Offset == GameManager.Instance.Passantable.transform.localPosition.z)
            {
                if (_xSelf * BoardState.Offset == GameManager.Instance.Passantable.transform.localPosition.x
                    && _xSelf!= _xPosition)
                {
                    MoveTracker.Instance.AddMove(_xPosition - _direction, _yPosition, -1, -1, GameManager.Instance.TurnCount);
                }
            }
        }

        BoardState.Instance.SetField(this, _xPosition, _yPosition);

        _hasMoved = true;
        GameManager.Instance.Passantable = null;
    }

}
