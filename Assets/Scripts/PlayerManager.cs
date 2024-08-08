using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rollSpeed;
    [SerializeField] float _jumpPower;
    [SerializeField] float _life;
    [SerializeField] float _attackInterval; //攻撃できるインターバル
    [SerializeField] float _attackComboTime; //攻撃のコンボが途切れるまでの時間
    [SerializeField] float _longRangeAttackInterval;
    [SerializeField] BoxCollider2D _boxCollider;
    [SerializeField] GameObject _longRangeAttackObject;
    [SerializeField] GameObject _longRangeAttackMuzzle;
    Rigidbody2D _rb2d;
    float _hMove;
    Animator _anim;
    SpriteRenderer _sprite;
    bool _isGround;
    bool _isAttack;
    bool _isBlock;
    bool _isDeath;
    public bool _isRoll;
    float _attackTime;
    float _rollTimer;
    float _longRangeAttackTimer;
    int _attackCount;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _longRangeAttackTimer = _longRangeAttackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        _rollTimer += Time.deltaTime;
        _longRangeAttackTimer += Time.deltaTime;
        if (_rollTimer > 0.25)
        {
            _isRoll = false;
        }
        if (!_isDeath)
        {
            _hMove = Input.GetAxisRaw("Horizontal");
            if (!_isBlock)
            {
                Jump();
                Attack();
                Move();
            }
            Block();
            if (_isAttack)
            {
                _boxCollider.enabled = true;
            }
            if (!_isAttack)
            {
                _boxCollider.enabled = false;
            }
        }
    }
    void Move()
    {
        _rb2d.AddForce(_hMove * _moveSpeed * Vector2.right, ForceMode2D.Force);
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRoll)
        {
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
        if (Input.GetMouseButtonDown(0) && _attackInterval + _attackTime < Time.time)
        {
            Debug.Log("a");
            _isAttack = true;
            _attackTime = Time.time;
            _attackCount++;
            _anim.SetTrigger("Attack");
        }
        else if (_attackInterval + _attackTime + _attackComboTime < Time.time)
        {
            _isAttack = false;
            _attackCount = 0;
        }
        if (_attackCount > 3)
        {
            _attackCount = 1;
        }
        if (Input.GetMouseButtonDown(2) && !_isRoll && !_isAttack && _longRangeAttackTimer > _longRangeAttackInterval)
        {
            _anim.Play("LongRangeAttack");
            _longRangeAttackTimer = 0;
            Instantiate(_longRangeAttackObject, _longRangeAttackMuzzle.transform.position, Quaternion.identity);
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
    public void Life(float plusLife, float minusLife)
    {
        var life = plusLife + -minusLife;
        _life += life;
        if (_life <= 0)
        {
            _anim.Play("Death");
            _isDeath = true;
        }
    }

    private void LateUpdate()
    {
        var offset = _boxCollider.offset;
        if (_hMove != 0)
        {
            _sprite.flipX = _hMove < 0;
        }
        if (_hMove < 0)
        {
            offset.x = -0.82f;
        }
        if (_hMove > 0)
        {
            offset.x = 0.82f;
        }
        _boxCollider.offset = offset;
        _anim.SetFloat("MoveX", Mathf.Abs(_rb2d.velocity.x));
        _anim.SetBool("IsGround", _isGround);
        _anim.SetFloat("MoveY", _rb2d.velocity.y);
        _anim.SetBool("IsBlock", _isBlock);
        _anim.SetBool("IsRoll", _isRoll);
        _anim.SetInteger("AttackCount", _attackCount);
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
}
