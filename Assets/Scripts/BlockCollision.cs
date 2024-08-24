using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour, IPause
{
    Goblin _goblin;
    PlayerManager _player;
    FallBlock _fallBlock;
    [SerializeField] float _destroyTime;
    BoxCollider2D _boxCollider;
    bool _isPause;
    bool _isBlockCollision;
    float _destroyTimer;
    //enumを使ってPlayerかGoblinかそれ以外かを判定したらいいのでは？←天才の発想
    // Start is called before the first frame update
    void Start()
    {
        _goblin = GetComponentInParent<Goblin>();
        _player = GetComponentInParent<PlayerManager>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _fallBlock = FindObjectOfType<FallBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != "Player")
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
            if (gameObject.tag == "Goblin")
            {
                _goblin.IsMove = false;
                _goblin.IsDead = true;
                _isBlockCollision = true;
                Debug.LogError("いってえなあこらあああ");
            }
            if (gameObject.tag == "Player")
            {
                _player.IsDeath = true;
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
