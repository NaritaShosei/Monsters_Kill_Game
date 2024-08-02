using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;
    Rigidbody2D _rb2d;
    float _hMove;
    Animator _anim;
    SpriteRenderer _sprite;
    bool _isGround;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }
    void Move()
    {
        _hMove = Input.GetAxisRaw("Horizontal");
        _rb2d.AddForce(_hMove * _moveSpeed * Vector2.right, ForceMode2D.Force);
    }
    void Jump()
    {
        Vector2 velocity = _rb2d.velocity;
        if (Input.GetKey(KeyCode.Space) && _isGround)
        {
            _isGround = false;
            velocity.y = _jumpPower;
        }
        _rb2d.velocity = velocity;
    }
    private void LateUpdate()
    {
        if (_hMove != 0)
        {
            _sprite.flipX = (_hMove < 0);
        }
        if (_anim)
        {
            _anim.SetFloat("MoveX", Mathf.Abs(_rb2d.velocity.x));
            _anim.SetBool("IsGround", _isGround);
            _anim.SetFloat("MoveY",_rb2d.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }
}
