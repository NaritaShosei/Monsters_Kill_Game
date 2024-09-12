using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] float _fadeTime;
    [SerializeField] Image _image;
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
    public void GetSceneChange()
    {
        //StartCoroutine(StartSceneChange(_sceneName, _waitTime));
        _image.gameObject.SetActive(true);
        _image.DOFade(1, _fadeTime).OnComplete(() => SceneChange(_sceneName));
    }
    static IEnumerator StartSceneChange(string sceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneChange(sceneName);
    }
}
