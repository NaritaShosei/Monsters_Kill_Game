using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongRangeAttack : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _destroyTime = 5;
    PlayerManager _player;
    Rigidbody2D _rb2d;
    float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _rb2d = GetComponent<Rigidbody2D>();
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = mousePos - _player.transform.position;
        _rb2d.velocity = (dir).normalized * _moveSpeed;
        transform.rotation = Quaternion.Euler(dir);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _destroyTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "LongRangeAttack")
        {
            Destroy(gameObject);
        }
    }
}
