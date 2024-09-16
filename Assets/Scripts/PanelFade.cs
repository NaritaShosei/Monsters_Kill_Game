using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PanelFade : MonoBehaviour
{
    [SerializeField] float _fadeTime;
    [SerializeField] string _sceneName;
    GameManager _gm;
    Text _text;
    Image _image;
    PlayerManager _player;
    bool _active = true;
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
                    _image.DOFade(1, _fadeTime).OnComplete(() => _text.DOFade(1, _fadeTime).OnComplete(() => SceneChangeManager.SceneChange(_sceneName)));
                }
                break;
        }
    }
}
