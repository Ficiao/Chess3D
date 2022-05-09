/*using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorController _whiteAnimator;
    [SerializeField]
    private AnimatorController _BlackAnimator;
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

    public AnimatorController Assign(SideColor _color)
    {
        if (_color == SideColor.White)
        {
            return _whiteAnimator;
        }
        else
        {
            return _BlackAnimator;
        }
    }
}*/
