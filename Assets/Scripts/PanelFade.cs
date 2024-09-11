using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    [SerializeField] float _fadeTime;
    [SerializeField] string _sceneName;
    Text _text;
    Image _image;
    PlayerManager _player;
    bool _active = true;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _image = GetComponent<Image>();
        _text = GameObject.Find("YOU DIED").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsDeath && _active)
        {
            _image.DOFade(1, _fadeTime).OnComplete(() => _text.DOFade(1, _fadeTime).OnComplete(() => SceneChangeManager.SceneChange(_sceneName)));
        }
    }
}
