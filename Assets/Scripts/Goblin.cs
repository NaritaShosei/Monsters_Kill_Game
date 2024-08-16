using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] float _destroyTime;
    [SerializeField] Vector2 _lineForWall;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] Vector2 _lineForGround;
    [SerializeField] LayerMask _groundLayer;
    Rigidbody2D _rb2d;
    Animator _animator;
    PlayerManager _player;
    SpriteRenderer _sr;
    bool _isDead;
    bool _isGround;
    bool _isMove = true;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            if (_isGround && _isMove)
            {
                Move();
            }
        }
    }
    void Move()
    {
        Vector2 start = this.transform.position;
        Debug.DrawLine(start, start + _lineForGround);
        Debug.DrawLine(start, start + _lineForWall);
        RaycastHit2D hit = Physics2D.Linecast(start, start + _lineForGround, _groundLayer);
        RaycastHit2D hit2 = Physics2D.Linecast(start, start + _lineForWall, _wallLayer);
        Vector2 velo = Vector2.zero;

        if (hit.collider || !hit2.collider)
        {
            velo = Vector2.right * _moveSpeed;
        }
        if (!hit.collider || hit2.collider)
        {
            _moveSpeed = -_moveSpeed;
            _lineForGround.x = -_lineForGround.x;
            _lineForWall = -_lineForWall;
        }

        velo.y = _rb2d.velocity.y;
        _rb2d.velocity = velo;
    }
    private void LateUpdate()
    {
        if (!_isDead)
        {
            _sr.flipX = _rb2d.velocity.x < 0; 
            _animator.SetFloat("XMove", Mathf.Abs(_rb2d.velocity.x));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block")
        {
            _isGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block")
        {
            _isGround = false;
        }
    }
    IEnumerator StartIsMove()
    {
        yield return new WaitForSeconds(1);
    }
}
