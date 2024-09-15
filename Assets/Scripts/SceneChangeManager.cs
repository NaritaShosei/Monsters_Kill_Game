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
    [SerializeField] string _sceneName;
    [SerializeField] float _fadeTime;
    [SerializeField] Image _image;
    [NonSerialized] public bool _isActive;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GetSceneChangeToTutorial()
    {
        //SceneChange("Stage" + int);
        // int += 1;
        if (_isActive)
        {
            _image.gameObject.SetActive(true);
            _image.DOFade(1, _fadeTime).OnComplete(() => SceneChange(_sceneName));
            _isActive = false;
        }
    }
}
