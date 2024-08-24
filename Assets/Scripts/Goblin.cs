using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Goblin : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] Vector2 _startLineForWallUpper;
    [SerializeField] Vector2 _lineForWallUpper;
    [SerializeField] Vector2 _startLineForWallDowner;
    [SerializeField] Vector2 _lineForWallDowner;
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
    float _animSpeed;
    public bool IsDead;
    bool _isGround;
    public bool IsMove = true;
    bool _isAttack;
    bool _isPause;
    Vector2 _start;
    Vector2 _deathPos;
    Vector2 _goblinVelocity;
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
        Debug.DrawLine(_start, _start + _lineForGround);
        Debug.DrawLine(_start + _startLineForWallUpper, _start + _lineForWallUpper);
        Debug.DrawLine(_start + _startLineForWallDowner, _start + _lineForWallDowner);
        if (!_isPause)
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
        }
        if (IsDead)
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
        RaycastHit2D hitGround = Physics2D.Linecast(_start, _start + _lineForGround, _groundLayer);
        RaycastHit2D hitWallUpper = Physics2D.Linecast(_start + _startLineForWallUpper, _start + _lineForWallUpper, _wallLayer);
        RaycastHit2D hitWallDowner = Physics2D.Linecast(_start + _startLineForWallDowner, _start + _lineForWallDowner, _wallLayer);
        Vector2 velo = Vector2.zero;

        if (hitGround.collider || !hitWallUpper.collider || !hitWallDowner)
        {
            velo = Vector2.right * _moveSpeed;
        }
        if (!hitGround.collider || hitWallUpper.collider || hitWallDowner)
        {
            _moveSpeed = -_moveSpeed;
            _lineForGround.x = -_lineForGround.x;
            _startLineForWallUpper.x = -_startLineForWallUpper.x;
            _startLineForWallDowner.x = -_startLineForWallDowner.x;
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
        if (!_isPause)
        {
            if (_rb2d.velocity.x != 0)
            {
                _sr.flipX = _rb2d.velocity.x < 0;
            }
            _animator.SetFloat("XMove", Mathf.Abs(_rb2d.velocity.x));
            _animator.SetBool("IsDead", IsDead);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsDead && collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block")
        {
            _isGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsDead && collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block")
        {
            _isGround = false;
        }
    }
    void IsAttackToTrue()// animation‚ÅŽg‚Á‚Ä‚éŠÖ”
    {
        _isAttack = true;
    }
    void IsAttackToFalse() // animation‚ÅŽg‚Á‚Ä‚éŠÖ”
    {
        _isAttack = false;
    }

    void IPause.Pause()
    {
        _isPause = true;
        _goblinVelocity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
        _animSpeed = _animator.speed;
        _animator.speed = 0;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
            | RigidbodyConstraints2D.FreezeRotation;
    }

    void IPause.Resume()
    {
        _isPause = false;
        _rb2d.velocity = _goblinVelocity;
        _animator.speed = _animSpeed;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
