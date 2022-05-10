using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorController _whiteAnimator;
    [SerializeField]
    private AnimatorController _blackAnimator;
    [SerializeField]
    private AnimatorController _whiteKnightAnimator;
    [SerializeField]
    private AnimatorController _blackKnightAnimator;
    [SerializeField]
    private AnimatorController _whiteBishopAnimator;
    [SerializeField]
    private AnimatorController _blackBishopAnimator;
    [SerializeField]
    private float _moveSpeed = 20f;
    [SerializeField]
    private AudioSource _moveSound;
    private bool _active = false;
    public bool Active { get => _active; }

    private static AnimationManager _instance;
    public static AnimationManager Instance { get => _instance; }

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

    public AnimatorController Assign(Piece _piece)
    {
        if (_piece.PieceColor == SideColor.White)
        {
            if(_piece is Knight || _piece is King)
            {
                return _whiteKnightAnimator;
            }

            if(_piece is Bishop)
            {
                return _whiteBishopAnimator;
            }

            return _whiteAnimator;
        }
        else
        {
            if (_piece is Knight || _piece is King)
            {
                return _blackKnightAnimator;
            }

            if (_piece is Bishop)
            {
                return _blackBishopAnimator;
            }

            return _blackAnimator;
        }
    }

    public void MovePiece(Piece _piece, Vector3 _target, Piece _killTarget)
    {
        _active = true;
        StartCoroutine(MoveAnimation(_piece, _target, _killTarget));
    }

    private IEnumerator MoveAnimation(Piece _piece, Vector3 _target, Piece _killTarget)
    {
        Animator _pieceAnimator = _piece.GetComponent<Animator>();

        _pieceAnimator.SetInteger("State", 1);
        while (_pieceAnimator.GetCurrentAnimatorStateInfo(0).IsName("Travel") == false)
        {
            yield return new WaitForSeconds(0.001f);
        }

        _target.y = _piece.transform.localPosition.y;
        while (_piece.transform.localPosition != _target)
        {
            _piece.transform.localPosition = Vector3.MoveTowards(_piece.transform.localPosition, _target, _moveSpeed * (Time.deltaTime));
            yield return new WaitForSeconds(0.001f);
        }

        _moveSound.Play();

        _pieceAnimator.SetInteger("State", 2);
        if (_killTarget != null)
        {
            _killTarget.Die();
        }

        while (_pieceAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartState") == false)
        {
            yield return new WaitForSeconds(0.001f);
        }

        _target.y = _piece.transform.localPosition.y;
        _piece.transform.localPosition = _target;
        _active = false;
    }
}
