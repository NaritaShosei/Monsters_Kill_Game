using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Goblin : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] float _life;
    [SerializeField] float _destroyTime;
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
    [SerializeField] BoxCollider2D _aliveCollider;
    [SerializeField] BoxCollider2D _deadCollider;
    [SerializeField] Image _hp;
    [SerializeField] Canvas _canvas;
    Rigidbody2D _rb2d;
    Animator _animator;
    [NonSerialized] public Animator _anim = default;
    PlayerManager _player;
    GameManager _gm;
    float _maxLife;
    float _attackTime;
    float _animSpeed;
    float _destroyTimer;
    public bool IsDeath;
    bool _isGround;
    public bool IsMove = true;
    bool _isAttack;
    bool _isPause;
    bool _isHit;
    bool _isInvisible;
    Vector2 _start;
    Vector2 _goblinVelocity;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerManager>();
        _gm = FindObjectOfType<GameManager>();
        _maxLife = _life;
        _anim = _animator;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(_start, _start + _lineForGround);
        Debug.DrawLine(_start + _startLineForWallUpper, _start + _lineForWallUpper);
        Debug.DrawLine(_start + _startLineForWallDowner, _start + _lineForWallDowner);
        if (_player.IsDeath)
        {
            _rb2d.velocity = Vector2.zero;
            _animator.speed = 0;
            _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
                | RigidbodyConstraints2D.FreezeRotation;
        }
        if (!_isPause && !_player.IsDeath)
        {
            if (!IsDeath && !_gm.IsMovie)
            {
                _start = transform.position;
                if (_isGround && IsMove && !_isHit)
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
                if (_life <= 0)
                {
                    IsDeath = true;
                }
            }
            if (IsDeath)
            {
                _destroyTimer += Time.deltaTime;
                if (_destroyTimer >= _destroyTime)
                {
                    Destroy(gameObject);
                }
                _aliveCollider.enabled = false;
                _deadCollider.enabled = true;
            }
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
        else if (!hitPlayer.collider && !_isInvisible)
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

        if (hitGround.collider || !hitWallUpper.collider || !hitWallDowner.collider)
        {
            velo = Vector2.right * _moveSpeed;
        }
        if (!hitGround.collider || hitWallUpper.collider || hitWallDowner.collider)
        {
            _moveSpeed = -_moveSpeed;
            _lineForGround.x = -_lineForGround.x;
            _startLineForWallUpper.x = -_startLineForWallUpper.x;
            _startLineForWallDowner.x = -_startLineForWallDowner.x;
            _lineForPlayer = -_lineForPlayer;
            _startLineForPlayer = -_startLineForPlayer;
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
            Vector2 canvasScale = _canvas.transform.localScale;
            canvasScale.x = -canvasScale.x;
            _canvas.transform.localScale = canvasScale;
        }

        velo.y = _rb2d.velocity.y;
        _rb2d.velocity = velo;
    }
    private void LateUpdate()
    {
        if (!_isPause && !_player.IsDeath)
        {
            _animator.SetFloat("XMove", Mathf.Abs(_rb2d.velocity.x));
            _animator.SetBool("IsDead", IsDeath);
            _animator.SetBool("IsHit", _isHit);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsDeath && collision.gameObject.layer == 10)
        {
            _isGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsDeath && collision.gameObject.layer == 10)
        {
            _isGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsDeath)
        {
            if (collision.gameObject.tag == "Player")
            {
                    _player.Life(-_attackDamage,_player._lifeReduceType = PlayerManager.LifeReduceType.enemy);
                    _player.BlockGauge(-1);
                
            }
            if (collision.gameObject.tag == "PlayerAttack" && _player.IsAttack)
            {
                if (!_isAttack)
                {
                    Life(-1);
                    _isHit = true;
                    _animator.Play("Hit");
                }
            }
        }
    }
    public void Life(float life)
    {
        DOTween.To(() => _life / _maxLife, x => _hp.fillAmount = x, (_life + life) / _maxLife, 0.5f);
        _life += life;
    }
    void IsHitFalse()
    {
        _isHit = false;
    }
    void IsAttackToTrue()// animationÇ≈égÇ¡ÇƒÇÈä÷êî
    {
        _isAttack = true;
    }
    void IsAttackToFalse() // animationÇ≈égÇ¡ÇƒÇÈä÷êî
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
    private void OnBecameInvisible()
    {
        IsMove = false;
        _isInvisible = true;
    }
    private void OnBecameVisible()
    {
        _isInvisible = false;
    }
}
