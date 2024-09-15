using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Sunrise : MonoBehaviour
{
    [SerializeField] float _completeTime;
    [SerializeField] float _waitTime;
    [SerializeField] Text _titleText;
    SceneChangeManager _sceneChangeManager;
    string _string;
    SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        _sceneChangeManager = FindObjectOfType<SceneChangeManager>();
        _string = _titleText.text;
        _titleText.text = "";
        _sr = GetComponent<SpriteRenderer>();
        transform.DOMoveY(1, _completeTime);
        //_sr.DOFade(1, _completeTime).OnComplete(() => _titleText.DOText(_string, _completeTime));
        _sr.DOFade(1, _completeTime).OnComplete(() => StartCoroutine(StartTitle()));
    }
    IEnumerator StartTitle()
    {
        for (var i = 0; i < _string.Length; i++)
        {
            yield return new WaitForSeconds(_waitTime / _string.Length);
            _titleText.text += _string[i].ToString();
        }
        _sceneChangeManager._isActive = true;
    }
}
