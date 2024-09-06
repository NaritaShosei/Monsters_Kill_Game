using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FallBlock : MonoBehaviour, IPause
{
    Rigidbody2D _rb2d;
    PlayerManager _player;
    public bool IsFall;
    Vector2 _fallBlockVelocity;
    // Start is called before the first frame update
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerManager>();
    }
    void Start()
    {
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
                | RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsDeath)
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
                | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "LongRangeAttack")
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezePositionX
           | RigidbodyConstraints2D.FreezeRotation;
            _rb2d.gravityScale = 1;
            IsFall = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation
           | RigidbodyConstraints2D.FreezePosition;
            IsFall = false;
        }
    }

    public void Pause()
    {
        _fallBlockVelocity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
            | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Resume()
    {
        _rb2d.velocity = _fallBlockVelocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
