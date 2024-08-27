using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : MonoBehaviour,IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] Vector2 _lineForWall;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] Vector2 _lineForGround;
    [SerializeField] LayerMask _groundLayer;
    bool _isPause;

    public void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!_isPause)
        {

        }
    }
    void FixedUpdate()
    {
        if (!_isPause)
        {

        }
    }

}
