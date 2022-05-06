using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
     public enum SideColor{
        White, 
        Black
    }

    [SerializeField]
    private SideColor _pieceColor;
    private Renderer _renderer;
    private Vector3 _startPosition;
    public Vector3 StartPosition { get => _startPosition; }
    private bool active;
    private Color _startColor;
    public SideColor PieceColor { get => PieceColor; }

    public abstract void CreatePath();

    private void Start()
    {
        _startPosition = transform.position;
        active = false;
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
    }

    private void OnMouseDown()
    {
        active = true;
        CreatePath();
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        if (active == false)
        {
            _renderer.material.color = _startColor;
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

}
