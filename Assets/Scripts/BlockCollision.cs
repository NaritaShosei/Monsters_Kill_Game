using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour
{
    Goblin _goblin;
    PlayerManager _player;
    FallBlock _fallBlock;
    BringerOfDeath _bringerOfDeath;
    [SerializeField] float _damage;
    [SerializeField] ParentType _parentType;
    [SerializeField] AudioSource _audio;
    BoxCollider2D _boxCollider;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            _fallBlock = collision.gameObject.GetComponent<FallBlock>();
            if (_fallBlock.IsFall)
            {
                Destroy(_boxCollider);
                _audio.Play();
                switch (_parentType)
                {
                    case ParentType.player:
                        _player.Life(-100,_player._lifeReduceType = PlayerManager.LifeReduceType.system);
                        break;
                    case ParentType.goblin:
                        _goblin.IsMove = false;
                        _goblin.Life(-1000);
                        _goblin._anim.Play("Death");
                        break;
                    case ParentType.bringer:
                        _bringerOfDeath.Life(-_damage);
                        _bringerOfDeath.IsHit = true;
                        Destroy(collision.gameObject);
                        break;
                }
            }
        }
    }
}
