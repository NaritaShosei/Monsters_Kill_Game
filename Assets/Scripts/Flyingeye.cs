using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyingeye : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] float _destroyTime;
    [SerializeField] BoxCollider2D _boxCollider;
    Rigidbody2D _rb2d;
    Animator _animator;
    PlayerManager _player;
    float _animSpeed;
    bool _isDead;
    bool _isPause;
    Vector2 _flyingeyeVelocity;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPause)
        {
            if (!_isDead)
            {
                var sin = Mathf.Sin(Time.time);
                transform.position += new Vector3(_moveSpeed * Time.deltaTime, sin * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDead)
        {
            if (collision.gameObject.tag == "Block")
            {
                _animator.Play("FlyingeyeHit");
                _isDead = true;
                _rb2d.gravityScale = 1;
                _boxCollider.enabled = true;
            }
            if (collision.gameObject.tag == "PlayerAttack")
            {
                if (_player.IsAttack)
                {
                    _animator.Play("FlyingeyeHit");
                    _isDead = true;
                    _rb2d.gravityScale = 1;
                    _boxCollider.enabled = true;
                }
            }
            else if (collision.gameObject.tag == "Player" && !_player.IsAttack)
            {
                _player.Life(-_attackDamage);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject, _destroyTime);
        }
    }

    public void Pause()
    {
        _isPause = true;
        _flyingeyeVelocity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
            | RigidbodyConstraints2D.FreezeRotation;
        _animSpeed = _animator.speed;
        _animator.speed = 0;
    }

    public void Resume()
    {
        _isPause = false;
        _rb2d.velocity = _flyingeyeVelocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator.speed = _animSpeed;
    }
}
