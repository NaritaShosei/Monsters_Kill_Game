using DG.Tweening;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoalBlock : MonoBehaviour, IPause
{
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] float _shakePower;
    [SerializeField] float _shakeTime;
    [SerializeField] float _targetPosition;
    [SerializeField] float _completeTime;
    [SerializeField] BoxCollider2D _collider;
    PlayerManager _player;
    TilemapRenderer _renderer;
    bool _active = true;
    bool _movie;
    GameManager _gm;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _gm = FindObjectOfType<GameManager>();
        _renderer = GetComponent<TilemapRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gm.IsClearConditions && _active)
        {
            StartCoroutine(StartDOMove());
            _active = false;
            var velo = _player._rb2d.velocity;
            velo.x = 0;
            _player._rb2d.velocity = velo;
            _player.IsStopping = true;
            _camera.Priority = 100;
        }
        if (_movie)
        {
            _player.IsStopping = false;
            _camera.Priority = 0;
            Destroy(_renderer);
            Destroy(_collider);
        }
    }
    IEnumerator StartDOMove()
    {
        yield return new WaitForSeconds(0.5f);
        transform.DOPunchPosition(new Vector3(0, _shakePower, 0), _shakeTime);
        transform.DOLocalMoveX(_targetPosition, _completeTime).OnComplete(() => _movie = true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_gm.IsClearConditions)
        {
            Debug.Log("GameClear");
        }
    }
    public void Pause()
    {
        transform.DOPause();
    }

    public void Resume()
    {
        transform.DOPlay();
    }
}