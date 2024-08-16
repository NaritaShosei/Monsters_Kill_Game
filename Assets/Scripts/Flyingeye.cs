using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyingeye : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackDamage;
    [SerializeField] float _destroyTime;
    [SerializeField] BoxCollider2D _boxCollider;
    Rigidbody2D _rb2d;
    Animator _animator;
    PlayerManager _player;
    bool _isDead;
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
        if (!_isDead)
        {
            var sin = Mathf.Sin(Time.time);
            transform.position += new Vector3(_moveSpeed * Time.deltaTime, sin * Time.deltaTime);
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
                _player.Life(_attackDamage);
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
}
