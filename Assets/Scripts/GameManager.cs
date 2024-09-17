using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform _bossStartPosition;
    CinemachineVirtualCamera _camera;
    bool _isPause;
    bool _isStart;
    public bool IsMovie;
    public bool IsClearConditions;
    float _animSpeed;
    PlayerManager _player;
    BringerOfDeath _bringer;
    [SerializeField] AudioSource _bgm;
    [SerializeField] float _fadeTime;
    [SerializeField] AudioSource _bossArearbgm;
    //List<AudioSource> _audio = new List<AudioSource>();
    //List<float> _audioTime = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _bringer = FindObjectOfType<BringerOfDeath>();
        _camera = GameObject.Find("Boss Camera").GetComponent<CinemachineVirtualCamera>();
        IsMovie = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !_player.IsDeath && !IsMovie)
        {
            PauseResume();
        }
        if (_bossStartPosition.transform.position.x <= _player.transform.position.x && !_isStart)
        {
            _isStart = true;
            _camera.Priority = 100;
            var velo = _player._rb2d.velocity;
            velo.x = 0;
            _player._rb2d.velocity = velo;
            _bringer._animator.Play("Cast-NoEffect");
            IsMovie = true;
            DOTween.To(() => 1f, x => _bgm.volume = x, 0f, _fadeTime).OnComplete(() => DOTween.To(() => 0f, x => _bossArearbgm.volume = x, 1f, _fadeTime).OnStart(() => _bossArearbgm.Play()));
        }
        if (IsMovie)
        {
            _player.IsStopping = true;
        }
        else
        {
            _player.IsStopping = false;
        }
    }
    void PauseResume()
    {
        _isPause = !_isPause;
        var obj = FindObjectsOfType<GameObject>();
        foreach (var objects in obj)
        {
            var pause = objects.GetComponent<IPause>();
            if (_isPause && pause != null)
            {
                pause.Pause();
            }
            else if (!_isPause && pause != null)
            {
                pause.Resume();
            }
        }
        //var audioSources = FindObjectsOfType<AudioSource>();
        //foreach (var audioSource in audioSources)
        //{
        //    if (audioSource.isPlaying && !_isPause)
        //    {
        //        _audio.Add(audioSource);
        //    }
        //    else if (_isPause)
        //    {
        //        _audio.Remove(audioSource);
        //    }
        //    foreach (var audio in _audio)
        //    {
        //        if (_isPause)
        //        {
        //            _audioTime.Add(audio.time);
        //        }
        //        else if (!_isPause)
        //        {
        //            _audioTime.Remove(audio.time);
        //        }
        //        foreach (var time in _audioTime)
        //        {
        //            if (_isPause)
        //            {
        //                audio.Pause();
        //            }
        //            else if (!_isPause)
        //            {
        //                audio.time = time;
        //                audio.Play();
        //            }
        //        }
        //    }
        //}
        var animObj = GameObject.FindGameObjectsWithTag("AnimationObject");
        foreach (var animObjects in animObj)
        {
            var animPause = animObjects.GetComponent<Animator>();
            if (_isPause)
            {
                _animSpeed = animPause.speed;
                animPause.speed = 0;
            }
            else if (!_isPause)
            {
                animPause.speed = _animSpeed;
            }
        }
    }
}
