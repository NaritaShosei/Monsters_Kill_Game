using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
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
    public void GetSceneChange(string sceneName,float waitTime)
    {
        StartCoroutine(StartSceneChange(sceneName, waitTime));
    }
      IEnumerator StartSceneChange(string sceneName , float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneChange(sceneName);
    }
}
