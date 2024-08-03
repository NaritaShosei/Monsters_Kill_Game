using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;
    [SerializeField] float _life;
    Rigidbody2D _rb2d;
    float _hMove;
    Animator _anim;
    SpriteRenderer _sprite;
    bool _isGround;
    bool _isAttack;
    bool _isBlock;
    float _attackTimer;
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
        if (!_isBlock)
        {
            Move();
            Jump();
            Attack();
        }
        Block();
    }
    void Move()
    {
        _hMove = Input.GetAxisRaw("Horizontal");
        _rb2d.AddForce(_hMove * _moveSpeed * Vector2.right, ForceMode2D.Force);
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
        _attackTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
        }
        else if (_attackTimer > 0.25)
        {
            _isAttack = false;
        }
    }
    void Block()
    {
        if (_isBlock)
        {
            _hMove = Input.GetAxisRaw("Horizontal");
        }
        if (Input.GetMouseButton(1))
        {
            _isBlock = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _isBlock = false;
        }
    }
    void Life(float plusLife, float minusLife)
    {
        var life = plusLife + -minusLife;
        _life += life;
    }

    private void LateUpdate()
    {
        if (_hMove != 0)
        {
            _sprite.flipX = _hMove < 0;
        }
        if (_anim)
        {
            _anim.SetFloat("MoveX", Mathf.Abs(_rb2d.velocity.x));
            _anim.SetBool("IsGround", _isGround);
            _anim.SetFloat("MoveY", _rb2d.velocity.y);
            _anim.SetBool("IsAttack", _isAttack);
            _anim.SetBool("IsBlock", _isBlock);
        }
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
}
