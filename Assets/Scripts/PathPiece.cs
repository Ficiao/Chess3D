using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PathSelect(Vector3 position);

public class PathPiece : MonoBehaviour
{
    public static event PathSelect PathSelect;
    [SerializeField]
    private string _name;
    public string Name { get => _name; }

    private Color _startColor;
    private Renderer _renderer;

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
        _renderer.material.color = _startColor;
        PathSelect?.Invoke(transform.localPosition);
    }

    private void Disable()
    {
        ObjectPool.Instance.RemoveHighlightPath(this);
    }

}
