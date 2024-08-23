using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobinBlockCollision : MonoBehaviour
{
    Goblin _goblin;
    [SerializeField] float _destroyTime;
    BoxCollider2D _boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        _goblin = GetComponentInParent<Goblin>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            _goblin.IsMove = false;
            _goblin.IsDead = true;
            Destroy(_goblin.gameObject, _destroyTime);
            Destroy(_boxCollider);
        }
    }
}
