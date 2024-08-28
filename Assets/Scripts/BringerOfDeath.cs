using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] float _life;
    [SerializeField] Vector2 _lineForWall;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] Vector2 _lineForGround;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Vector2 _lineForPlayer;
    [SerializeField] Vector2 _startLineForPlayer;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] Animator _anim;
    [SerializeField] BoxCollider2D _attackCollider;
    bool _isPause;
    bool _isGround;
    bool _isMove;
    public bool IsAttack;
    bool _isDeath;
    public bool IsHit;
    PlayerManager _player;
    Rigidbody2D _rb2d;
    Vector2 _start;


    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Debug.DrawLine(_start, _start + _lineForGround);
        Debug.DrawLine(_start, _start + _lineForWall);
        if (_player.IsDeath)
        {
            _anim.speed = 0;
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
                | RigidbodyConstraints2D.FreezeRotation;
        }
        if (!_isPause && !_player.IsDeath && !_isDeath)
        {
            _anim.SetBool("IsHIt", IsHit);
            _start = transform.position;
            Attack();
            if (_isGround && _isMove && !IsHit)
            {
                Move();
            }
            if (IsAttack)
            {
                _attackCollider.enabled = true;
            }
            if (!IsAttack)
            {
                _attackCollider.enabled = false;
            }
        }
    }
    void FixedUpdate()
    {
        if (!_isPause && !_player.IsDeath && !_isDeath)
        {
            _anim.SetFloat("MoveX", Mathf.Abs(_rb2d.velocity.x));
        }
    }

    void Attack()
    {
        Debug.DrawLine(_start + _startLineForPlayer, _start + _lineForPlayer);
        RaycastHit2D hitPlayer = Physics2D.Linecast(_start + _startLineForPlayer, _start + _lineForPlayer, _playerLayer);
        if (hitPlayer.collider)
        {
            _isMove = false;
            _anim.SetTrigger("Attack");
        }
        else
        {
            _isMove = true;
        }
    }
    void Move()
    {
        Vector2 velo = _rb2d.velocity;
        RaycastHit2D hitWall = Physics2D.Linecast(_start, _start + _lineForWall, _wallLayer);
        RaycastHit2D hitGround = Physics2D.Linecast(_start, _start + _lineForGround, _groundLayer);
        if (!hitWall.collider || hitGround.collider)
        {
            velo.x = -_moveSpeed;
        }
        if (hitWall.collider || !hitGround.collider)
        {
            _lineForWall = -_lineForWall;
            _lineForGround.x = -_lineForGround.x;
            _moveSpeed = -_moveSpeed;
            Vector2 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
            _startLineForPlayer = -_startLineForPlayer;
            _lineForPlayer = -_lineForPlayer;
        }
        velo.y = _rb2d.velocity.y;
        _rb2d.velocity = velo;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDeath)
        {
            if (collision.gameObject.tag == "Player")
            {
                _player.Life(-_attackDamage);
            }
            if (collision.gameObject.tag == "PlayerAttack" && _player.IsAttack)
            {
                if (!IsAttack)
                {
                    _life -= 1;
                    IsHit = true;
                    _anim.Play("Hurt");
                }
            }
        }
    }
    public void Pause()
    {
        _isPause = true;
        _anim.speed = 0;
    }

    public void Resume()
    {
        _isPause = false;
    }
}
