using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
    [SerializeField] float _fallPower = 10;
    Rigidbody2D _rb2d;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Fall()
    {
        _rb2d.velocity = Vector2.down * _fallPower;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "LongRangeAttack")
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezePositionX
           | RigidbodyConstraints2D.FreezeRotation;
            Fall();
        }
        else
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation
           | RigidbodyConstraints2D.FreezePosition;
        }
    }
}
