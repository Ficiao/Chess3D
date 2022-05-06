using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Selected(Piece self);

public abstract class Piece : MonoBehaviour
{
     public enum SideColor{
        White, 
        Black
    }

    [SerializeField]
    private SideColor _pieceColor;
    public SideColor PieceColor { get => _pieceColor; }
    private Renderer _renderer;
    private Vector3 _startPosition;
    public Vector3 StartPosition { get => _startPosition; }
    private bool _active;
    public bool Active { get => _active; set { _active = false; _renderer.material.color = _startColor; } }
    private Color _startColor;

    public static event Selected Selected;

    public abstract void CreatePath();

    private void Start()
    {
        _startPosition = transform.position;
        _active = false;
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
    }

    private void OnMouseDown()
    {
        _active = true;
        Selected?.Invoke(this);
        CreatePath();
        _renderer.material.color = Color.yellow;
    }

    private void OnMouseEnter()
    {
        if (PieceController.Instance.AnyActive == false)
        {
            _renderer.material.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        if (_active == false)
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
        BoardState.Instance.SetField(this, (int)(_position.x / 1.5f), (int)(_position.z / 1.5f));
        transform.localPosition = _position;
    }

}
