using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BringerOfDeath : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    public float _life;
    [SerializeField] float _attackInterval;
    [SerializeField] float _longRangeAttackDistance;
    [SerializeField] float _longRangeAttackInterval = 5f;
    [SerializeField] Vector2 _lineForWall;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] Vector2 _lineForGround;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Vector2 _lineForPlayer;
    [SerializeField] Vector2 _startLineForPlayer;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] Animator _anim;
    [SerializeField] BoxCollider2D _attackCollider;
    [SerializeField] GameObject _object;
    [SerializeField] Image _hp;
    [SerializeField] Canvas _canvas;
    float _longRangeAttackTimer;
    float _maxLife;
    bool _isPause;
    bool _isGround;
    public bool IsStopping = true;
    bool IsMove;
    public bool IsAttack;
    [NonSerialized] public bool IsAttacking;
    bool _isDeath;
    public bool IsHit;
    bool _isInstantiate = true;
    public bool IsLongRangeAttacking = true;
    //bool _active = true;
    float _attackTime;
    float _animSpeed;
    float _distance;
    [NonSerialized] public Animator _animator;
    PlayerManager _player;
    Rigidbody2D _rb2d;
    Vector2 _start;
    Vector2 _bringerVelocity;


    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = _anim;
        _maxLife = _life;
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
        if (!_isPause && !_player.IsDeath && !IsStopping)
        {
            _distance = Vector2.Distance(_player.transform.position, transform.position);
            _longRangeAttackTimer += Time.deltaTime;
            if (!_isDeath)
            {
                _anim.SetBool("IsHIt", IsHit);
                _start = transform.position;
                Attack();
                if (_isGround && IsMove && !IsHit)
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
                if (_life <= 0)
                {
                    _isDeath = true;
                }
                if (_longRangeAttackTimer >= _attackInterval)
                {
                    IsLongRangeAttacking = true;
                    //_active = true;
                }
            }
            if (_isDeath)
            {
                if (_isInstantiate)
                {
                    Instantiate(_object, transform.position, Quaternion.identity);
                    _isInstantiate = false;
                }
                _anim.Play("Death");
            }
        }
    }
    void FixedUpdate()
    {
        if (!_isPause && !_player.IsDeath && !_isDeath)
        {
            _anim.SetFloat("MoveX", Mathf.Abs(_rb2d.velocity.x));
            _anim.SetBool("IsLongRangeAttacking", IsLongRangeAttacking);
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
                _anim.SetTrigger("Attack");
                _attackTime = Time.time;
            }
        }
        else
        {
            IsMove = true;
        }
        if ((_distance > _longRangeAttackDistance && IsLongRangeAttacking))
        {
            //_active = false;
            IsMove = false;
            _anim.SetTrigger("LongRangeAttack");
            _longRangeAttackTimer = 0;
            //if (_player.transform.position.x - transform.position.x < 0)
            //{
            //    transform.localRotation = Quaternion.Euler(0, 0, 0);
            //}
            //else
            //{
            //    transform.localRotation = Quaternion.Euler(0, 180, 0);
            //}
        }
        //if (_longRangeAttackTimer > 6)
        //{
        //    _active = false;
        //    IsMove = false;
        //    _anim.SetTrigger("LongRangeAttack");
        //    _longRangeAttackTimer = 0;
        //}
    }
    void Move()
    {
        Vector2 velo = _rb2d.velocity;
        RaycastHit2D hitWall = Physics2D.Linecast(_start, _start + _lineForWall, _wallLayer);
        RaycastHit2D hitGround = Physics2D.Linecast(_start, _start + _lineForGround, _groundLayer);
        if (!hitWall.collider || hitGround.collider)
        {
            velo.x = -_moveSpeed;
            //velo.x = (transform.localRotation.y / 180) % 2 == 0? -_moveSpeed : _moveSpeed;
        }
        if (hitWall.collider || !hitGround.collider)
        {
            _lineForWall = (transform.localRotation.y / 180) % 2 == 0 ? -_lineForWall : _lineForWall;
            _lineForGround.x = (transform.localRotation.y / 180) % 2 == 0 ? -_lineForGround.x : _lineForGround.x;
            _startLineForPlayer = (transform.localRotation.y / 180) % 2 == 0 ? -_startLineForPlayer : _lineForPlayer;
            _lineForPlayer = (transform.localRotation.y / 180) % 2 == 0 ? -_lineForPlayer : _lineForPlayer;
            //transform.localRotation = Quaternion.Euler(0, transform.localRotation.y + 180, 0);
            _moveSpeed = -_moveSpeed;
            Vector2 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
            Vector2 canvasScale = _canvas.transform.localScale;
            canvasScale.x = -canvasScale.x;
            _canvas.transform.localScale = canvasScale;
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
                _player.Life(-_attackDamage, _player._lifeReduceType = PlayerManager.LifeReduceType.enemy);
                _player.BlockGauge(-1);
            }
            if (collision.gameObject.tag == "PlayerAttack" && _player.IsAttack)
            {
                if (!IsAttack && !IsAttacking)
                {
                    _life -= 1;
                    DOTween.To(() => _life / _maxLife, x => _hp.fillAmount = x, _life / _maxLife, 0.3f);
                    IsHit = true;
                }
            }
        }
    }
    public void Pause()
    {
        _isPause = true;
        _animSpeed = _anim.speed;
        _anim.speed = 0;
        _bringerVelocity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
            | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Resume()
    {
        _isPause = false;
        _anim.speed = _animSpeed;
        _rb2d.velocity = _bringerVelocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
