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
        var button = FindObjectsOfType<Button>();
        foreach (var buttons in button)
        {
            var image = buttons.GetComponent<Image>();
            var text = buttons.GetComponentInChildren<Text>();
            image.DOFade(0,0);
            text.DOFade(0,0);
        }
        _sceneChangeManager = FindObjectOfType<SceneChangeManager>();
        _string = _titleText.text;
        _titleText.text = "";
        _sr = GetComponent<SpriteRenderer>();
        transform.DOMoveY(1, _completeTime);
        //_sr.DOFade(1, _completeTime).OnComplete(() => _titleText.DOText(_string, _completeTime));
        _sr.DOFade(1, _completeTime).OnComplete(() =>
        {
            StartCoroutine(StartTitle());
            var button = FindObjectsOfType<Button>();
            foreach (var buttons in button)
            {
                var image = buttons.GetComponent<Image>();
                var text = buttons.GetComponentInChildren<Text>();
                image.DOFade(1, _completeTime);
                text.DOFade(1, _completeTime);
            }
        });
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
