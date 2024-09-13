using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour, IPause
{
    Goblin _goblin;
    PlayerManager _player;
    FallBlock _fallBlock;
    BringerOfDeath _bringerOfDeath;
    [SerializeField] float _destroyTime;
    [SerializeField] float _damage;
    [SerializeField] ParentType _parentType;
    BoxCollider2D _boxCollider;
    bool _isPause;
    bool _isBlockCollision;
    float _destroyTimer;
    enum ParentType
    {
        player,
        goblin,
        bringer
    }

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
            case ParentType.bringer:
                _bringerOfDeath = GetComponentInParent<BringerOfDeath>();
                break;
        }
        _boxCollider = GetComponent<BoxCollider2D>();
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
        if (collision.gameObject.tag == "Block")
        {
            _fallBlock = collision.gameObject.GetComponent<FallBlock>();
            if (_fallBlock.IsFall)
            {
                Destroy(_boxCollider);
                switch (_parentType)
                {
                    case ParentType.player:
                        _player.Life(-100,_player._lifeReduceType = PlayerManager.LifeReduceType.system);
                        break;
                    case ParentType.goblin:
                        _goblin.IsMove = false;
                        _goblin.IsDeath = true;
                        _isBlockCollision = true;
                        _goblin._anim.Play("Death");
                        break;
                    case ParentType.bringer:
                        _bringerOfDeath._life -= _damage;
                        _bringerOfDeath.IsHit = true;
                        Destroy(collision.gameObject);
                        break;
                }
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
