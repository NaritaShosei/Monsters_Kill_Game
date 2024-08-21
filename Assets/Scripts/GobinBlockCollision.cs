using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobinBlockCollision : MonoBehaviour
{
    Goblin _goblin;
    [SerializeField] float _destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        _goblin = GetComponentInParent<Goblin>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            _goblin.IsMove = false;
            _goblin.IsDead = true;
            Destroy(_goblin.gameObject, _destroyTime);
        }
    }
}
