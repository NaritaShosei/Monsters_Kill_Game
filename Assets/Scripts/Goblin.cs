using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Goblin : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] float[] _randomWaitTime;
    [SerializeField] Vector2 _lineForWall;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] Vector2 _lineForGround;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Vector2 _lineForPlayer = new Vector2(1, 0);
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float _attackInterval;
    Rigidbody2D _rb2d;
    Animator _animator;
    PlayerManager _player;
    SpriteRenderer _sr;
    float _attackTimer;
    int _randomIndex;
    public bool IsDead;
    bool _isGround;
    public bool IsMove = true;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _sr = GetComponent<SpriteRenderer>();
        StartCoroutine(StartIsMove());
    }

    // Update is called once per frame
    void Update()
    {
        _attackTimer += Time.deltaTime;
        if (!IsDead)
        {
            if (_isGround && IsMove)
            {
                Move();
            }
            Attack();
        }
    }
    void Attack()
    {
        Vector2 start = transform.position;
        Debug.DrawLine(start, start + _lineForPlayer);
        RaycastHit2D hitPlayer = Physics2D.Linecast(start, start + _lineForPlayer, _playerLayer);
        if (hitPlayer.collider && _attackTimer >= _attackInterval)
        {
            _animator.SetTrigger("Attack");
            _attackTimer = 0;
        }
    }
    void Move()
    {
        Vector2 start = transform.position;
        Debug.DrawLine(start, start + _lineForGround);
        Debug.DrawLine(start, start + _lineForWall);
        RaycastHit2D hitGround = Physics2D.Linecast(start, start + _lineForGround, _groundLayer);
        RaycastHit2D hitWall = Physics2D.Linecast(start, start + _lineForWall, _wallLayer);
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
    IEnumerator StartIsMove()
    {
        _randomIndex = Random.Range(0, _randomIndex);
        yield return new WaitForSeconds(_randomWaitTime[_randomIndex]);
        IsMove = true;
        _randomIndex = Random.Range(0, _randomIndex);
        yield return new WaitForSeconds(_randomWaitTime[_randomIndex]);
        IsMove = false;
        StartCoroutine(StartIsMove());
    }
}
