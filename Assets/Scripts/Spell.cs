using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour,IPause
{
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] float _attackDamage;
    PlayerManager _player;
    Animator _anim;
    float _animSpeed;
    bool _isDamage = true;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _anim = GetComponent<Animator>();
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ColliderTrue()
    {
        _collider.enabled = true;
    }
    void ColliderFalse()
    {
        _collider.enabled = false;
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isDamage)
        {
            _player.Life(-_attackDamage);
            _isDamage = false;
        }
    }

    public void Pause()
    {
        _animSpeed = _anim.speed;
        _anim.speed = 0;
    }

    public void Resume()
    {
        _anim.speed = _animSpeed;
    }
}
