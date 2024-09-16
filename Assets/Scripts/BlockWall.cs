using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockWall : MonoBehaviour
{
    [SerializeField] GameObject _startPosition;
    FallBlock _fallBlock;
    PlayerManager _player;
    BoxCollider2D[] _boxCollider;
    TilemapRenderer _tilemapRenderer;
    bool _isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        _fallBlock = FindObjectOfType<FallBlock>();
        _player = FindObjectOfType<PlayerManager>();
        _boxCollider = GetComponents<BoxCollider2D>();
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _tilemapRenderer.enabled = false;
        foreach (var boxCollider in _boxCollider)
        {
            boxCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (!_fallBlock.IsFall)
        {
            if (_player.transform.position.x >= _startPosition.transform.position.x && _isActive)
            {
                foreach (var boxCollider in _boxCollider)
                {
                    boxCollider.enabled = true;
                }
                _tilemapRenderer.enabled = true;
                _isActive = false;
            }
        }
    }
}
