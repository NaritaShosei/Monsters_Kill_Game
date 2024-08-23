using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Goblin : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] Vector2 _lineForWall;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] Vector2 _lineForGround;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Vector2 _lineForPlayer = new Vector2(1, 0);
    [SerializeField] Vector2 _startLineForPlayer;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float _attackInterval;
    [SerializeField] BoxCollider2D _attackCollider;
    [SerializeField] BoxCollider2D _boxCollider;
    Rigidbody2D _rb2d;
    Animator _animator;
    PlayerManager _player;
    SpriteRenderer _sr;
    float _attackTime;
    public bool IsDead;
    bool _isGround;
    public bool IsMove = true;
    bool _isAttack;
    Vector2 _start;
    Vector2 _deathPos;
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
        if (!IsDead)
        {
            _start = transform.position;
            if (_isGround && IsMove)
            {
                Move();
            }
            Attack();
            if (_isAttack)
            {
                _attackCollider.enabled = true;
            }
            if (!_isAttack)
            {
                _attackCollider.enabled = false;
            }
            _deathPos = transform.position;
        }
        else
        {
            _animator.Play("Goblin_Death");
            _boxCollider.enabled = false;
            transform.position = _deathPos;
        }
    }
    void Attack()
    {
        Debug.DrawLine(_start + _startLineForPlayer, _start + _lineForPlayer);
        RaycastHit2D hitPlayer = Physics2D.Linecast(_start + _startLineForPlayer, _start + _lineForPlayer, _playerLayer);
        if (hitPlayer.collider)
        {
            IsMove = false;
            if (_attackTime + _attackInterval < Time.time)
            {
                _animator.SetTrigger("Attack");
                _attackTime = Time.time;
            }
        }
        else
        {
            IsMove = true;
        }
    }
    void Move()
    {
        Debug.DrawLine(_start, _start + _lineForGround);
        Debug.DrawLine(_start, _start + _lineForWall);
        RaycastHit2D hitGround = Physics2D.Linecast(_start, _start + _lineForGround, _groundLayer);
        RaycastHit2D hitWall = Physics2D.Linecast(_start, _start + _lineForWall, _wallLayer);
        Vector2 velo = Vector2.zero;

        if (hitGround.collider || !hitWall.collider)
        {
            velo = Vector2.right * _moveSpeed;
        }
        if (!hitGround.collider || hitWall.collider)
        {
            _moveSpeed = -_moveSpeed;
            _lineForGround.x = -_lineForGround.x;
            _lineForWall = -_lineForWall;
            _lineForPlayer = -_lineForPlayer;
            _startLineForPlayer = -_startLineForPlayer;
            Vector2 offset = _attackCollider.offset;
            offset.x = -offset.x;
            _attackCollider.offset = offset;
        }

        velo.y = _rb2d.velocity.y;
        _rb2d.velocity = velo;
    }
    private void LateUpdate()
    {
        if (_rb2d.velocity.x != 0)
        {
            _sr.flipX = _rb2d.velocity.x < 0;
        }
        _animator.SetFloat("XMove", Mathf.Abs(_rb2d.velocity.x));
        _animator.SetBool("IsDead", IsDead);
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
    void IsAttackToTrue()
    {
        _isAttack = true;
    }
    void IsAttackToFalse()
    {
        _isAttack = false;
    }
}
