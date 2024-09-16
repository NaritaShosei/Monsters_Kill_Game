using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField] string[] _sceneName;
    [SerializeField] float _fadeTime;
    [SerializeField] Image _image;
    [NonSerialized] public bool _isActive = true;
    public static int _sceneIndexLength;
    static int _sceneIndexCount = -1;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioSource _bgm;
    [SerializeField] GameType _gameType;
    enum GameType
    {
        title,
        nextstage
    }

    void Start()
    {
        _sceneIndexLength = _sceneName.Length;
        switch (_gameType)
        {
            case GameType.title:
                break;

            case GameType.nextstage:
                _sceneIndexCount += 1;
                if (_sceneIndexCount > _sceneName.Length)
                {
                    _sceneIndexCount = 0;
                }
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GetNextStageSceneChange()
    {
        if (_isActive)
        {
            DOTween.To(() => 1f, x => _bgm.volume = x, 0f, _fadeTime);
            _audio.Play();
            _isActive = false;
            Debug.Log(_sceneName[_sceneIndexCount]);
            _image.gameObject.SetActive(true);
            _image.DOFade(1, _fadeTime).OnComplete(() =>
            {
                SceneChange(_sceneName[_sceneIndexCount]);
            });
        }
    }
    public void GetSceneChange(string sceneName)
    {
        if (_isActive)
        {
            DOTween.To(() => 1f, x => _bgm.volume = x, 0f, _fadeTime);
            _audio.Play();
            _image.gameObject.SetActive(true);
            _image.DOFade(1, _fadeTime).OnComplete(() =>
            {
                SceneChange(sceneName);
            });
            _isActive = false;
        }
    }
}
