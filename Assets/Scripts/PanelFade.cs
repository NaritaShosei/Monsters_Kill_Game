using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    [SerializeField] float _fadeTime;
    [SerializeField] string _sceneName;
    Image _image;
    PlayerManager _player;
    bool _active = true;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsDeath && _active)
        {
            //_image.DOFade(100, _fadeTime).OnComplete(() => SceneChangeManager.SceneChange(_sceneName));
            _image.DOFade(1, _fadeTime).OnComplete(() => Debug.Log("YOU DIED"));
            //_image.DOFade(0, _fadeTime).OnComplete(() => _image.DOFade(1, _fadeTime).OnComplete(()=>Debug.Log("\\;")));
        }
    }
}
