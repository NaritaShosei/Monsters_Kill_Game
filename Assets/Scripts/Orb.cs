using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Orb : MonoBehaviour, IPause
{
    [SerializeField] float _speed;
    [SerializeField] float _magnetDistance = 3;
    PlayerManager _player;
    GameManager _gm;
    Rigidbody2D _rb2d;
    Vector2 _orbVelocity;
    bool _isPause;
    [SerializeField] AudioSource _audio;
    bool _active = true;

    public void Pause()
    {
        _orbVelocity = _rb2d.velocity;
        _rb2d.velocity = Vector2.zero;
        _isPause = true;
    }

    public void Resume()
    {
        _rb2d.velocity = _orbVelocity;
        _isPause = false;
    }

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
        if (!_isPause)
        {
            if (dis < _magnetDistance)
            {
                _rb2d.velocity = ((_player.transform.position + new Vector3(0, 0.5f) - transform.position).normalized * _speed);
            }
            if (dis < 0.5f && _active)
            {
                _rb2d.velocity = Vector3.zero;
                var child = gameObject.transform.GetChild(0);
                child.gameObject.SetActive(false);
                _gm.IsClearConditions = true;
                _audio.Play();
                _active = false;
                //var child = gameObject.transform.GetComponentsInChildren<Transform>();
                //foreach (var children in child)
                //{
                //    // children.DOFade(0, 0.5f);
                //    var material = children.GetComponent<Material>();
                //    if (material && _active)
                //    {
                //        _active = false;    
                //        material.DOFade(0, 0.5f).OnComplete(() => _gm.IsClearConditions = true);
                //    }
                //}
            }
        }
    }
}
