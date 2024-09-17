using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rollSpeed;
    [SerializeField] float _rollInterval;
    [SerializeField] float _jumpPower;
    [SerializeField] float _life;
    [SerializeField] float _attackInterval; //攻撃できるインターバル
    [SerializeField] float _attackComboTime; //攻撃のコンボが途切れるまでの時間
    [SerializeField] float _longRangeAttackInterval;
    [SerializeField] float _isAttackTime;
    [SerializeField] float _isHitTime;
    [SerializeField] float _blockCount;
    [SerializeField] BoxCollider2D _attackCollider;
    [SerializeField] GameObject _longRangeAttackObject;
    [SerializeField] GameObject _longRangeAttackMuzzle;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] Image _hp;
    [SerializeField] Image _blockGauge;
    Vector3 _mousePosition;
    [NonSerialized] public Rigidbody2D _rb2d;
    float _hMove;
    Animator _anim;
    SpriteRenderer _sprite;
    bool _isGround;
    [NonSerialized] public bool IsAttack;
    bool _isBlocking;
    [NonSerialized] public bool IsDeath;
    bool _isRoll;
    bool _isHit;
    bool _isPause;
    bool _isWall;
    [NonSerialized] public bool IsStopping;
    bool _fallDead;
    bool _isBlockCondition = true;
    float _attackTime;
    float _rollTime;
    float _rollTimer;
    float _longRangeAttackTimer;
    float _animSpeed;
    float _maxLife;
    float _maxCount;
    int _attackCount;
    Vector2 _muzzlePos;
    Vector2 _playerVelocity;
    Vector2 _deadPosition;
    [SerializeField] AudioSource _attackAudio;
    [SerializeField] AudioSource _blockAudio;
    [SerializeField] AudioSource _blockBrakeAudio;
    [SerializeField] AudioSource _healGaugeAudio;
    [SerializeField] AudioSource _deadAudio;
    [SerializeField] AudioSource _hitAudio;
    [SerializeField] AudioSource _rollAudio;
    [SerializeField] AudioSource _jumpAudio;
    [SerializeField] AudioSource _blockStartAudio;
    [NonSerialized] public LifeReduceType _lifeReduceType;
    public enum LifeReduceType
    {
        enemy,
        system
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _longRangeAttackTimer = _longRangeAttackInterval;
        _muzzlePos = _longRangeAttackMuzzle.transform.position;
        _maxLife = _life;
        _maxCount = _blockCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPause)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _rollTimer += Time.deltaTime;
            _longRangeAttackTimer += Time.deltaTime;
            if (_rollTimer > 0.25)
            {
                _isRoll = false;
            }
            if (!IsDeath && !IsStopping)
            {
                _deadPosition = transform.position;
                _hMove = Input.GetAxisRaw("Horizontal");
                Block();
                if (!_isBlocking)
                {
                    Jump();
                    Attack();
                    //Roll↓
                    if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRoll && (_rollInterval + _rollTime < Time.time))
                    {
                        _rollAudio.Play();
                        _rollTime = Time.time;
                        _isRoll = true;
                        var rollSpeed = _rollSpeed * (_sprite.flipX ? -1 : 1);
                        _rb2d.AddForce(Vector2.right * rollSpeed, ForceMode2D.Impulse);
                        _rollTimer = 0;
                    }
                }
                if (IsAttack)
                {
                    _attackCollider.enabled = true;
                }
                if (!IsAttack)
                {
                    _attackCollider.enabled = false;
                }
                if (_isGround)
                {
                    _isWall = false;
                }

            }
            else if (IsDeath)
            {
                _anim.Play("Death");
                Vector2 pos = transform.position;
                pos.x = _deadPosition.x;
                transform.position = pos;
                if (!_fallDead)
                {
                    _camera.Priority = 1000;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isPause)
        {
            if (!IsDeath)
            {
                if (!_isBlocking && !IsStopping)
                {
                    _rb2d.AddForce(Vector2.right * _hMove * _moveSpeed, ForceMode2D.Force);
                }
            }
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _jumpAudio.Play();
            _isGround = false;
            _rb2d.AddForce(_jumpPower * Vector2.up, ForceMode2D.Impulse);
        }
    }
    void Attack()
    {
        if (!_isHit)
        {
            if (Input.GetButtonDown("Fire1") && _attackInterval + _attackTime < Time.time)
            {
                _attackTime = Time.time;
                _attackCount++;
                _anim.SetTrigger("Attack");
            }
            else if (_attackInterval + _attackTime + _attackComboTime < Time.time)
            {
                _attackCount = 0;
            }
            if (_attackCount > 3)
            {
                _attackCount = 1;
            }
            if (Input.GetButtonDown("Fire3") && !_isRoll && !IsAttack && _longRangeAttackTimer > _longRangeAttackInterval)
            {
                _anim.Play("LongRangeAttack");
                _longRangeAttackTimer = 0;
                var longRangeAttackDirection = transform.position.x;
                _sprite.flipX = _mousePosition.x < longRangeAttackDirection;
                if (_mousePosition.x < longRangeAttackDirection)
                {
                    _muzzlePos.x = transform.position.x - 0.69f;
                }
                if (_mousePosition.x > longRangeAttackDirection)
                {
                    _muzzlePos.x = transform.position.x + 0.69f;
                }
                _longRangeAttackMuzzle.transform.position = _muzzlePos;
                Instantiate(_longRangeAttackObject, _longRangeAttackMuzzle.transform.position, Quaternion.identity);
            }
        }
    }
    void Block()
    {
        if (Input.GetButtonDown("Fire2") && _isBlockCondition)
        {
            _blockStartAudio.Play();
        }
        if (Input.GetButton("Fire2") && _isBlockCondition)
        {
            _isBlocking = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            _isBlocking = false;
        }
    }
    public void Life(float life, LifeReduceType type)
    {
        if (!_isPause)
        {
            float currentLife = _life;
            switch (_lifeReduceType)
            {
                case LifeReduceType.enemy:
                    if (!_isRoll && !IsAttack && !_isBlocking)
                    {
                        LifeSystem(life);
                        _hitAudio.Play();
                    }
                    break;
                case LifeReduceType.system:
                    LifeSystem(life);
                    break;
            }

        }
    }
    void LifeSystem(float life)
    {
        float currentLife = _life;
        DOTween.To(() => currentLife / _maxLife, x => _hp.fillAmount = x, (currentLife + life) / _maxLife, 0.3f);
        _life += life;
        if (_life <= 0)
        {
            IsDeath = true;
        }
        else
        {
            _anim.Play("Hit");
            _isHit = true;
            StartCoroutine(StartIsHitFalse());
        }
    }
    IEnumerator StartIsHitFalse()
    {
        yield return new WaitForSeconds(_isHitTime);
        _isHit = false;
    }
    public void BlockGauge(float value)
    {
        if (_isBlocking)
        {
            float currentGauge = _blockCount;
            DOTween.To(() => currentGauge / _maxCount, x => _blockGauge.fillAmount = x, (currentGauge + value) / _maxCount, 0.3f);
            _blockCount += value;
            if (_blockCount <= 0)
            {
                _blockCount = 0;
                _isBlockCondition = false;
                _isBlocking = false;
                StartCoroutine(StartBlockConditionTrue());
                _blockBrakeAudio.Play();
            }
            else
            {
                _blockAudio.Play();
            }
        }
    }
    IEnumerator StartBlockConditionTrue()
    {
        yield return new WaitForSeconds(2);
        if (!IsDeath)
        {
            _healGaugeAudio.Play();
            float currentGauge = _blockCount;
            DOTween.To(() => currentGauge / _maxCount, x => _blockGauge.fillAmount = x, (currentGauge + _maxCount) / _maxCount, 0.3f);
            _blockCount = _maxCount;
            _isBlockCondition = true;
        }
    }
    private void LateUpdate()
    {
        if (!_isPause)
        {
            if (_hMove != 0)
            {
                _sprite.flipX = _hMove < 0;
            }
            if (!_sprite.flipX)
            {
                _muzzlePos.x = transform.position.x + 0.69f;
            }
            if (_sprite.flipX)
            {
                _muzzlePos.x = transform.position.x - 1f;
            }
            _muzzlePos.y = transform.position.y + 0.6f;
            _longRangeAttackMuzzle.transform.position = _muzzlePos;
            _anim.SetFloat("MoveX", Mathf.Abs(_rb2d.velocity.x));
            _anim.SetBool("IsGround", _isGround);
            _anim.SetFloat("MoveY", _rb2d.velocity.y);
            _anim.SetBool("IsBlock", _isBlocking);
            _anim.SetBool("IsRoll", _isRoll);
            _anim.SetBool("IsWall", _isWall);
            _anim.SetInteger("AttackCount", _attackCount);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            Life(-100, _lifeReduceType = LifeReduceType.system);
            _fallDead = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            _isGround = true;
        }
        if (collision.gameObject.layer == 9)
        {
            _isWall = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            _isGround = false;
        }
        if (collision.gameObject.tag == "Wall")
        {
            _isWall = false;
        }
    }

    void IPause.Pause()
    {
        _isPause = true;
        _playerVelocity = _rb2d.velocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
            | RigidbodyConstraints2D.FreezeRotation;
        _animSpeed = _anim.speed;
        _anim.speed = 0;
    }

    void IPause.Resume()
    {
        _isPause = false;
        _rb2d.velocity = _playerVelocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _anim.speed = _animSpeed;
        if (_isBlocking)
        {
            _isBlocking = false;
        }
    }
    void IsAttackTrue()
    {
        IsAttack = true;
        _attackAudio.Play();
    }
    void IsAttackFalse()
    {
        IsAttack = false;
    }
    void Dead()
    {
        if (!_fallDead)
        {
            _deadAudio.Play();
        }
    }
}
