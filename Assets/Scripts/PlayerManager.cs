using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
    [SerializeField] BoxCollider2D _attackCollider;
    [SerializeField] GameObject _longRangeAttackObject;
    [SerializeField] GameObject _longRangeAttackMuzzle;
    Vector3 _longRangeAttackDirection = new Vector3(0, 0, -10);
    Vector3 _mousePosition;
    Rigidbody2D _rb2d;
    float _hMove;
    Animator _anim;
    SpriteRenderer _sprite;
    bool _isGround;
    public bool IsAttack;
    bool _isBlock;
    public bool IsDeath;
    bool _isRoll;
    bool _isHit;
    bool _isPause;
    float _attackTime;
    float _rollTime;
    float _rollTimer;
    float _longRangeAttackTimer;
    float _animSpeed;
    int _attackCount;
    Vector2 _muzzlePos;
    Vector2 _playerVelocity;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _longRangeAttackTimer = _longRangeAttackInterval;
        _muzzlePos = _longRangeAttackMuzzle.transform.position;
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
            if (!IsDeath)
            {
                _hMove = Input.GetAxisRaw("Horizontal");
                if (!_isBlock)
                {
                    Jump();
                    Attack();
                }
                Block();
                if (IsAttack)
                {
                    _attackCollider.enabled = true;
                }
                if (!IsAttack)
                {
                    _attackCollider.enabled = false;
                }
            }
            else if (IsDeath)
            {
                _anim.Play("Death");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isPause)
        {
            if (!IsDeath)
            {
                if (!_isBlock)
                {
                    Move();
                }
            }
        }
    }

    void Move()
    {
        _rb2d.AddForce(Vector2.right * _hMove * _moveSpeed, ForceMode2D.Force);
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRoll && _rollInterval + _rollTime < Time.time)
        {
            _rollTime = Time.time;
            _isRoll = true;
            var rollSpeed = _rollSpeed * (_sprite.flipX ? -1 : 1);
            _rb2d.AddForce(Vector2.right * rollSpeed, ForceMode2D.Impulse);
            _rollTimer = 0;
        }
    }
    void Jump()
    {
        Vector2 velocity = _rb2d.velocity;
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _isGround = false;
            velocity.y = _jumpPower;
        }
        _rb2d.velocity = velocity;
    }
    void Attack()
    {
        if (!_isHit)
        {
            if (Input.GetMouseButtonDown(0) && _attackInterval + _attackTime < Time.time)
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
            if (Input.GetMouseButtonDown(2) && !_isRoll && !IsAttack && _longRangeAttackTimer > _longRangeAttackInterval)
            {
                _anim.Play("LongRangeAttack");
                _longRangeAttackTimer = 0;
                _sprite.flipX = _mousePosition.x < _longRangeAttackDirection.x;
                if (_mousePosition.x < _longRangeAttackDirection.x)
                {
                    _muzzlePos.x = transform.position.x - 0.69f;
                }
                if (_mousePosition.x > _longRangeAttackDirection.x)
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
        if (Input.GetMouseButton(1))
        {
            _isBlock = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _isBlock = false;
        }
    }
    public void Life(float life)
    {
        if (!_isPause)
        {
            if (!_isBlock && !_isRoll && !IsAttack)
            {
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
        }
    }
    IEnumerator StartIsHitFalse()
    {
        yield return new WaitForSeconds(_isHitTime);
        _isHit = false;
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
            _anim.SetBool("IsBlock", _isBlock);
            _anim.SetBool("IsRoll", _isRoll);
            _anim.SetInteger("AttackCount", _attackCount);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            _isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            _isGround = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
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

    void IPause.Pause()
    {
        _isPause = true;
        _playerVelocity = _rb2d.velocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
            | RigidbodyConstraints2D.FreezeRotation;
        _animSpeed = _anim.speed;
        _anim.speed = 0;
        Debug.Log("THE WORLD");
    }

    void IPause.Resume()
    {
        _isPause = false;
        _rb2d.velocity = _playerVelocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _anim.speed = _animSpeed;
        if (_isBlock)
        {
            _isBlock = false;
        }
    }
    void IsAttackTrue()
    {
        IsAttack = true;
    }
    void IsAttackFalse()
    {
        IsAttack = false;
    }
}
