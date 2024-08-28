using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour, IPause
{
    Goblin _goblin;
    PlayerManager _player;
    FallBlock _fallBlock;
    [SerializeField] float _destroyTime;
    [SerializeField] ParentType _parentType;
    BoxCollider2D _boxCollider;
    bool _isPause;
    bool _isBlockCollision;
    float _destroyTimer;
    enum ParentType
    {
        player,
        goblin
    }

    //enumを使ってPlayerかGoblinかそれ以外かを判定したらいいのでは？←天才の発想
    // Start is called before the first frame update
    void Start()
    {
        switch (_parentType)
        {
            case ParentType.player:
                _player = GetComponentInParent<PlayerManager>();
                break;
            case ParentType.goblin:
                _goblin = GetComponentInParent<Goblin>();
                break;
        }
        _boxCollider = GetComponent<BoxCollider2D>();
        _fallBlock = FindObjectOfType<FallBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_parentType == ParentType.goblin)
        {
            if (!_isPause && _isBlockCollision)
            {
                _destroyTimer += Time.deltaTime;
            }
            if (_destroyTimer >= _destroyTime)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block" && _fallBlock.IsFall)
        {
            Destroy(_boxCollider);
            switch (_parentType)
            {
                case ParentType.player:
                    _player.IsDeath = true;
                    break;
                case ParentType.goblin:
                    _goblin.IsMove = false;
                    _goblin.IsDeath = true;
                    _isBlockCollision = true;
                    _goblin._anim.Play("Death");
                    break;
            }
        }
    }

    void IPause.Pause()
    {
        _isPause = true;
    }

    void IPause.Resume()
    {
        _isPause = false;
    }
}
