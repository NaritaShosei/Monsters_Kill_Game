using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
public class FallBlock : MonoBehaviour, IPause
{
    Rigidbody2D _rb2d;
    PlayerManager _player;
    public bool IsFall;
    Vector2 _fallBlockVelocity;
    [SerializeField] BlockType _blockType;
    [SerializeField] GameObject _startPosition;
    bool _isActive = true;
    bool _isPause;
    // Start is called before the first frame update
    enum BlockType
    {
        manual,
        automatic
    }

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerManager>();
    }
    void Start()
    {
        _rb2d.constraints = RigidbodyConstraints2D.FreezePosition
                | RigidbodyConstraints2D.FreezeRotation;
        _rb2d.gravityScale = 0;
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
        if (!_player.IsDeath && !_isPause)
        {
            switch (_blockType)
            {
                case BlockType.automatic:
                    if (_startPosition.transform.position.x <= _player.transform.position.x && _isActive)
                    {
                        IsFall = true;
                        _isActive = false;
                    }
                    break;
            }
            if (IsFall)
            {
                _rb2d.constraints = RigidbodyConstraints2D.FreezePositionX
                    | RigidbodyConstraints2D.FreezeRotation;
                _rb2d.gravityScale = 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "LongRangeAttack" && _blockType == BlockType.manual)
        {
            IsFall = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
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
        _isPause = true;
    }

    public void Resume()
    {
        _rb2d.velocity = _fallBlockVelocity;
        _rb2d.constraints = RigidbodyConstraints2D.FreezePositionX
           | RigidbodyConstraints2D.FreezeRotation;
        _isPause = false;
    }
}
