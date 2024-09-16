using DG.Tweening;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System;

public class GoalBlock : MonoBehaviour, IPause
{
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] float _shakePower;
    [SerializeField] float _shakeTime;
    [SerializeField] float _targetPosition;
    [SerializeField] float _completeTime;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] Image _image;
    [SerializeField] Text _text;
    [SerializeField] string _sceneName;
    PlayerManager _player;
    TilemapRenderer _renderer;
    bool _active = true;
    bool _movie;
    GameManager _gm;
    string _textString;
    [SerializeField] AudioSource _bossArearAudio;
    [SerializeField] AudioSource _goalBlockAudio;
    [SerializeField] AudioSource _goalAudio;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _gm = FindObjectOfType<GameManager>();
        _renderer = GetComponent<TilemapRenderer>();
        _textString = _text.text;
        _text.text = "";
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
            _gm.IsMovie = true;
            _camera.Priority = 100;
        }
        if (_movie)
        {
            _gm.IsMovie = false;
            _camera.Priority = 0;
            Destroy(_renderer);
            Destroy(_collider);
        }
    }
    IEnumerator StartDOMove()
    {
        yield return new WaitForSeconds(0.5f);
        _goalBlockAudio.DOFade(0, 2).OnStart(() => _goalBlockAudio.Play());
        transform.DOPunchPosition(new Vector3(0, _shakePower, 0), _shakeTime);
        transform.DOLocalMoveX(_targetPosition, _completeTime).OnComplete(() => _movie = true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_gm.IsClearConditions)
        {
            //_text.DOText
            _image.DOFade(1, 2).OnComplete(() =>
            {
                _text.DOText(_textString, 2).OnStart(() => _goalAudio.Play());
                DOTween.To(() => 0f, x => _goalAudio.volume = x, 1f, 3).OnComplete(() => SceneChangeManager.SceneChange(_sceneName));
            });
            DOTween.To(() => 1f, x => _bossArearAudio.volume = x, 0f, 3);
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
