using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _magnetDistance = 3;
    PlayerManager _player;
    GameManager _gm;
    Rigidbody2D _rb2d;

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<PlayerManager>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var dis = Vector2.Distance(transform.position, _player.transform.position + new Vector3(0, 0.5f));
        if (dis < _magnetDistance)
        {
            _rb2d.velocity = ((_player.transform.position + new Vector3(0, 0.5f) - transform.position).normalized * _speed);
        }
        if (dis < 0.2f)
        {
            _rb2d.velocity = Vector3.zero;
            var child = gameObject.transform.GetChild(0);
            child.gameObject.SetActive(false);
            _gm.IsClearConditions = true;
          
            Debug.Log(dis);
        }
    }
}
