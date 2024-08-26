using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LongRangeAttack : MonoBehaviour, IPause
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _destroyTime = 5;
    PlayerManager _player;
    Rigidbody2D _rb2d;
    float _timer;
    bool _isPause;
    Vector2 _longRangeAttackVelcity;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _rb2d = GetComponent<Rigidbody2D>();
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePos - _player.transform.position).normalized;
        _rb2d.velocity = dir * _moveSpeed;
        transform.rotation = Quaternion.Euler(dir);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsDeath)
        {
            _rb2d.velocity = Vector2.zero;
        }
        if (!_isPause && !_player.IsDeath)
        {
            _timer += Time.deltaTime;
            if (_timer >= _destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        _isPause = true;
        _longRangeAttackVelcity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
    }

    public void Resume()
    {
        _isPause = false;
        _rb2d.velocity = _longRangeAttackVelcity;
    }
}
