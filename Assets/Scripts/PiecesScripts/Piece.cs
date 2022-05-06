using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
     public enum SideColor{
        White, 
        Black
    }

    private SideColor _pieceColor;
    private Renderer _renderer;
    private Color _startColor;
    public SideColor PieceColor { get => PieceColor; }

    public abstract void Move();
    public abstract void Die();
    public abstract void Revive();

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        Debug.Log(_renderer.material.color);
        _startColor = _renderer.material.color;
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = Color.yellow;
    }
    private void OnMouseExit()
    {
        _renderer.material.color = _startColor;
    }

}
