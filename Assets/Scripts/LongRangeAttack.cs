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
    Vector2 _longRangeAttackVelocity;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _rb2d = GetComponent<Rigidbody2D>();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)_player.transform.position).normalized;
        _rb2d.velocity = dir * _moveSpeed;
        transform.localRotation = Quaternion.LookRotation(Vector3.forward, dir);
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
        Destroy(gameObject);
        Debug.Log(collision.gameObject.name);
    }

    public void Pause()
    {
        _isPause = true;
        _longRangeAttackVelocity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
    }

    public void Resume()
    {
        _isPause = false;
        _rb2d.velocity = _longRangeAttackVelocity;
    }
}
