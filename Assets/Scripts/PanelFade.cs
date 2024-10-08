using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    [SerializeField] float _fadeTime;
    [SerializeField] string _sceneName;
    GameManager _gm;
    Text _text;
    Image _image;
    PlayerManager _player;
    bool _active = true;
    [SerializeField] AudioSource _bgm;
    [SerializeField] AudioSource _bossArearbgm;
    [SerializeField] AudioSource _gameOverAudio;
    [SerializeField] GameType _gameType;
    enum GameType
    {
        ingame,
        title
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (_gameType)
        {
            case GameType.ingame:
                _player = FindObjectOfType<PlayerManager>();
                _gm = FindObjectOfType<GameManager>();
                _image = GetComponent<Image>();
                _text = GameObject.Find("YOU DIED").GetComponent<Text>();
                _image.DOFade(0, _fadeTime).OnComplete(() => _gm.IsMovie = false);
                break;
            case GameType.title:
                _image = GetComponent<Image>();
                _image.DOFade(0, _fadeTime).OnComplete(() => _image.gameObject.SetActive(false));
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (_gameType)
        {
            case GameType.ingame:
                if (_player.IsDeath && _active)
                {
                    DOTween.To(() => 1f, x => _bgm.volume = x, 0f, _fadeTime);
                    DOTween.To(() => 1f, x => _bossArearbgm.volume = x, 0f, _fadeTime);
                    _image.DOFade(1, _fadeTime).OnComplete(() =>
                    {
                        DOTween.To(() => 0f, x => _gameOverAudio.volume = x, 1f, 4).OnComplete(() => SceneChangeManager.SceneChange(_sceneName));
                        _text.DOFade(1, _fadeTime).OnStart(() => _gameOverAudio.Play());
                    });
                    _active = false;
                }
                break;
        }
    }
}
